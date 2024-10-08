using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEngine.UIElements.Experimental;
//using SFB; // Cần có plugin StandaloneFileBrowser

public class UploadAndDisplayImage : MonoBehaviour
{
    public RawImage avatarImage;
    public Button getIMG;// Image UI để hiển thị ảnh sau khi upload
    private string filePath;      // Đường dẫn tới file ảnh
    private string apiUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9JLxC73TpfJyOtIMrVmSLcfrOpJdFsLNlFA&s";  // Đường dẫn tới API
    private void Awake()
    {
        getIMG.onClick.AddListener(() =>
        {
            StartCoroutine(LoadImage(apiUrl));
        });
        
    }
    // Hàm để mở hộp thoại chọn file
    /* public void SelectImage()
     {
         // Mở hộp thoại chọn file (lấy đường dẫn của file ảnh)
         var paths = StandaloneFileBrowser.OpenFilePanel("Chọn ảnh", "", "png", false);

         if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
         {
             filePath = paths[0]; // Lưu đường dẫn file
             StartCoroutine(UploadImage());  // Bắt đầu upload ảnh
         }
     }*/

    // Hàm upload ảnh lên API
    IEnumerator UploadImage()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File không tồn tại tại: " + filePath);
            yield break;
        }

        // Đọc file thành byte array
        byte[] fileData = File.ReadAllBytes(filePath);

        // Tạo form kiểu multipart/form-data
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, "upload.png", "image/png");

        // Tạo yêu cầu POST để gửi file tới API
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, form);

        // Gửi yêu cầu và chờ kết quả
        yield return request.SendWebRequest();

        // Kiểm tra nếu upload thành công
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload thành công: " + request.downloadHandler.text);

            // Sau khi upload thành công, có thể lấy đường dẫn ảnh từ API
            string imageUrl = request.downloadHandler.text;

            // Bắt đầu tải ảnh về để hiển thị
           // StartCoroutine(LoadImage(imageUrl));
        }
        else
        {
            Debug.LogError("Lỗi upload: " + request.error);
        }
    }

    // Hàm tải ảnh từ server và hiển thị lên màn hình
    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // Chờ tải về ảnh từ URL
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Lấy texture từ phản hồi
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Hiển thị texture lên UI RawImage
            avatarImage.texture = texture;

            // Tính toán tỷ lệ và điều chỉnh ảnh để nằm gọn trong RawImage
            ScaleImageToFitRawImage(texture);
        }
        else
        {
            Debug.LogError("Lỗi khi tải ảnh: " + request.error);
        }
    }

    // Hàm để điều chỉnh ảnh sao cho nằm gọn trong RawImage mà không thay đổi kích thước của RawImage
    private void ScaleImageToFitRawImage(Texture2D texture)
    {
        // Lấy kích thước của RawImage
        RectTransform rt = avatarImage.GetComponent<RectTransform>();
        float rawImageWidth = rt.rect.width;
        float rawImageHeight = rt.rect.height;

        // Lấy kích thước của ảnh
        float imageWidth = texture.width;
        float imageHeight = texture.height;

        // Tính toán tỷ lệ width/height của RawImage và của ảnh
        float rawImageAspect = rawImageWidth / rawImageHeight;
        float imageAspect = imageWidth / imageHeight;

        // Điều chỉnh kích thước của ảnh để vừa với RawImage nhưng giữ nguyên tỷ lệ ảnh
        if (imageAspect > rawImageAspect)
        {
            // Ảnh rộng hơn so với RawImage, điều chỉnh theo chiều rộng
            float scale = rawImageWidth / imageWidth;
            rt.sizeDelta = new Vector2(rawImageWidth, imageHeight * scale);
        }
        else
        {
            // Ảnh cao hơn so với RawImage, điều chỉnh theo chiều cao
            float scale = rawImageHeight / imageHeight;
            rt.sizeDelta = new Vector2(imageWidth * scale, rawImageHeight);
        }
    }
}
