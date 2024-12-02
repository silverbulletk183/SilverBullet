using UnityEngine;
using UnityEngine.UI; // Th? vi?n cho Button

public class SkinSwapController : MonoBehaviour
{
    public Material[] materials; // M?ng ch?a các material
    private SkinnedMeshRenderer skinnedMeshRenderer; // Component SkinnedMeshRenderer
    private int currentMaterialIndex = 0;

    public Button buttonA; // Nút ?? chuy?n sang Material 1

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // ??t material ban ??u (n?u có)
        if (materials.Length > 0)
        {
            skinnedMeshRenderer.material = materials[currentMaterialIndex];
        }

        // G?n s? ki?n OnClick cho Button A
        if (buttonA != null)
        {
            buttonA.onClick.AddListener(ChangeToMaterial1);
        }
    }

    // Ph??ng th?c thay ??i sang Material 1
    public void ChangeToMaterial1()
    {
        // ??m b?o Material 1 t?n t?i và ch? ??i n?u ch?a ??i
        if (materials.Length > 1 && skinnedMeshRenderer.material != materials[1])
        {
            currentMaterialIndex = 1; // Ch? s? c?a Material 1
            skinnedMeshRenderer.material = materials[currentMaterialIndex];
        }
    }
}
