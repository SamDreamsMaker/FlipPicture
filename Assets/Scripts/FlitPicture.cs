using Aspose.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FlitPicture : MonoBehaviour
{
    // Start is called before the first frame update
    public void FlipPictures()
    {
        List<string> extensions = new List<string>();
        extensions.Add("*.png");
        extensions.Add("*.jpg");
        extensions.Add("*.PNG");
        extensions.Add("*.JPG");
        extensions.Add("*.jpeg");
        extensions.Add("*.JPEG");

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
