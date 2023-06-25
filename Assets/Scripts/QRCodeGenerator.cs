using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage rawImageReceiver;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField bioInputField;
    [SerializeField] private TMP_InputField avatarIDInputField;
    [SerializeField] private TMP_InputField linkedInInputField;
    [SerializeField] private TMP_InputField websteInputField;

    private Texture2D encodedTexture;

   
    void Start()
    {
        encodedTexture = new Texture2D(256, 256);
    }

    private Color32[] Encode(string encodingText, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(encodingText);
    }

    private void EncodeTextToQRCode()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            ShowInputError(nameInputField, "Please enter a name.");
            return;
        }

        RemoveInputErrors(nameInputField);

        BusinessCardInfo businessCard = new BusinessCardInfo(
            nameInputField.text,
            bioInputField.text,
            avatarIDInputField.text,
            linkedInInputField.text,
            websteInputField.text
            );

        string businessCardInfoText = businessCard.SaveToString();
        
        Debug.Log("Object info: " + businessCard.m_name + " " + businessCard.m_bio + " " + businessCard.m_avatarID + " " + businessCard.m_linkedIn + " " + businessCard.m_website);
        Debug.Log("JSON info: " + businessCardInfoText);

        Color32[] convertPixelToTexture = Encode(businessCardInfoText, encodedTexture.width, encodedTexture.height);
        encodedTexture.SetPixels32(convertPixelToTexture);
        encodedTexture.Apply();

        rawImageReceiver.texture = encodedTexture;
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }

    private void ShowInputError(TMP_InputField inputField, string errorMessage)
    {
        TMP_Text placeholder = inputField.placeholder.GetComponent<TMP_Text>();
        placeholder.text = errorMessage;
        inputField.GetComponentInParent<Image>().color = Color.red;
    }

    private void RemoveInputErrors(TMP_InputField inputField)
    {
        inputField.GetComponentInParent<Image>().color = Color.white;
    }
}
