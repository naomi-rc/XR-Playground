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
    [SerializeField] private TMP_InputField textInputField;

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

    private void EncodeTextToQRCode ()
    {
        string textWrite = string.IsNullOrEmpty(textInputField.text) ? "Enter some text" : textInputField.text;
        Color32[] convertPixelToTexture = Encode(textWrite, encodedTexture.width, encodedTexture.height);
        encodedTexture.SetPixels32(convertPixelToTexture);
        encodedTexture.Apply();

        rawImageReceiver.texture = encodedTexture;
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }
}
