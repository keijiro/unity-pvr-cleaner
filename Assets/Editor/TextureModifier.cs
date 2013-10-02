using UnityEngine;
using UnityEditor;
using System.Collections;

class TextureModifier : AssetPostprocessor
{
    void OnPreprocessTexture ()
    {
        var importer = (assetImporter as TextureImporter);

        importer.textureType = TextureImporterType.GUI;
		importer.compressionQuality = (int)TextureCompressionQuality.Best;

        if (assetPath.EndsWith ("PVR.png")) {
            importer.textureFormat = TextureImporterFormat.RGBA32;
        }
	}

    void OnPostprocessTexture (Texture2D texture)
    {
        if (!assetPath.EndsWith ("PVR.png")) {
            return;
        }

        var pixels = texture.GetPixels ();

        for (var i = 0; i < pixels.Length; i++) {
            if (pixels [i].a == 0.0f) {
                pixels [i] = new Color (0.5f, 0.5f, 0.5f, 0.0f);
            }
        }

        texture.SetPixels (pixels);
        EditorUtility.CompressTexture (texture, TextureFormat.PVRTC_RGBA4, TextureCompressionQuality.Best);
    }
}