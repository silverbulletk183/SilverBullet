
using SFB;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadFile : MonoBehaviour
{
    public Button uploadButton;
    public Button confirmUploadButton; // Nút để xác nhận upload
    public RawImage imagePreview; // RawImage để hiển thị hình ảnh
    string apiUrl = "http://localhost:3000/api/userimage?id=67245dd4928d539b560b2761";

    private string selectedFilePath;

    void Start()
    {
        uploadButton.onClick.AddListener(OnUploadButtonClick);
        confirmUploadButton.onClick.AddListener(OnConfirmUpload);
       // confirmUploadButton.interactable = false; // Vô hiệu hóa nút upload ban đầu
    }

    private void OnUploadButtonClick()
    {
        // Open file selection window, allowing only image files
        var extensions = new[] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
    };
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
    }

    public void WriteResult(string[] paths)
    {
        if (paths.Length == 0)
        {
            Debug.LogWarning("No files selected.");
            return;
        }

        foreach (var path in paths)
        {
            Debug.Log("Selected file: " + path);
            StartCoroutine(LoadImage(path));
            selectedFilePath = path;// Load each image in a separate coroutine
        }
        
    }

    private IEnumerator LoadImage(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is null or empty.");
            yield break;
        }

        // Validate the file path
        if (!IsPathValid(filePath))
        {
            Debug.LogError("Invalid file path: " + filePath);
            yield break;
        }

        try
        {
            // Create a Texture2D from the image file
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image into Texture2D

            imagePreview.texture = texture; // Assign Texture to RawImage
            ScaleImageToFitRawImage(texture); // Set RawImage size to match the image
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading image: " + ex.Message);
        }

        yield return null; // Wait for one frame to display
    }

    // Function to validate the file path
    private bool IsPathValid(string path)
    {
        char[] invalidChars = System.IO.Path.GetInvalidPathChars();
        return path.IndexOfAny(invalidChars) < 0;
    }

    private void OnConfirmUpload()
    {
        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            StartCoroutine(Upload(selectedFilePath));
        }
    }

   /* private IEnumerator Upload(string filePath)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        WWWForm form = new WWWForm();

        // Determine MIME type based on file extension
        string mimeType = GetMimeType(filePath);
        form.AddBinaryData("image", fileData, System.IO.Path.GetFileName(filePath), mimeType);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Upload failed: " + www.error);
            }
            else
            {
                Debug.Log("File uploaded successfully: " + www.downloadHandler.text);
            }
        }
    }*/
    public IEnumerator Upload(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        WWWForm form = new WWWForm();

        // Determine MIME type based on file extension
        string mimeType = GetMimeType(filePath);
        form.AddBinaryData("image", fileData, Path.GetFileName(filePath), mimeType);

        string uploadUrl = apiUrl;


        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            www.certificateHandler = new BypassCertificateHandler();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Upload failed: " + www.error);
            }
            else
            {
                Debug.Log("File uploaded successfully: " + www.downloadHandler.text);
            }
        }
    }

    private string GetMimeType(string filePath)
    {
        string extension = System.IO.Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".gif":
                return "image/gif";
            case ".bmp":
                return "image/bmp";
            case ".tiff":
                return "image/tiff";
            case ".webp":
                return "image/webp";
            default:
                return "application/octet-stream"; // Generic binary data
        }
    }


    private void ScaleImageToFitRawImage(Texture2D texture)
    {
        // Lấy kích thước của RawImage
        RectTransform rt = imagePreview.GetComponent<RectTransform>();
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
    public class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // Bỏ qua kiểm tra chứng chỉ
        }
    }
}
