using Aspose.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FlitPicture : MonoBehaviour
{
    [SerializeField]
    GameObject message;
    // Start is called before the first frame update
    public void FlipPictures()
    {
        DisableMessage();
        List<string> extensions = new List<string>();
        extensions.Add("*.png");
        extensions.Add("*.jpg");
        extensions.Add("*.jpeg");
        extensions.Add("*.bmp");
        extensions.Add("*.tiff");
        extensions.Add("*.apng");
        extensions.Add("*.jpeg2000");
        extensions.Add("*.gif");

        if (Directory.Exists(Application.streamingAssetsPath)) {
            string worldsFolder = Application.streamingAssetsPath;
            Debug.Log(worldsFolder);
            DirectoryInfo d = new DirectoryInfo(worldsFolder);
            foreach (string e in extensions) {
                foreach (FileInfo file in d.GetFiles(e, SearchOption.AllDirectories)) {
                    Flip(file.FullName, file.Name, file.Extension, "_flip");
                }
            }
        }
        message.SetActive(true);
        Invoke("DisableMessage", 5.0f);
    }
    void DisableMessage() {
        message.SetActive(false);
    }
    // Update is called once per frame
    void Flip(string fullpath, string nameFile,string extension,string suffix)
    {
        Debug.Log(fullpath);
        using (Image image = Image.Load(fullpath)) {
            // Flip the image
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            // Save image
            image.Save(fullpath + suffix + extension);
        }
    }

}
