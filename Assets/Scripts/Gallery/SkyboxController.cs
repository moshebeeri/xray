// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SkyboxController : MonoBehaviour
{
    string first_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0016.JPG?alt=media&token=c0412b8b-a107-429f-9ddb-0f9bdd63c619";
    string next_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0023.JPG?alt=media&token=2c23e798-ddd6-4cdf-9492-5e7da180fea0";
    int CubemapResolution = 2048;

    private Texture2D source;

    /// <summary>
    /// These are the faces of a cube
    /// </summary>
    private Vector3[][] faces =
    {
        new Vector3[] {
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f)
        }
    };

    public void next()
    {
        Debug.Log("new 360 image");
        StartCoroutine(setImage(next_url));
    }

    void Start()
    {
        // start Coroutine to handle the WWW asynchronous process
        StartCoroutine(setImage(first_url));
    }

    IEnumerator setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        //Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        source = DownloadHandlerTexture.GetContent(request);

        //request.LoadImageIntoTexture(source);


        // new cubemap
        Cubemap cube = new Cubemap(CubemapResolution, TextureFormat.RGBA32, false);

        Color[] CubeMapColors;

        for (int i = 0; i < 6; i++)
        {
            CubeMapColors = CreateCubemapTexture(CubemapResolution, (CubemapFace)i);
            cube.SetPixels(CubeMapColors, (CubemapFace)i);
        }
        // we set the cubemap from the texture pixel by pixel
        cube.Apply();

        // Destroy all unused textures
        DestroyImmediate(source);
        Texture2D[] texs = FindObjectsOfType<Texture2D>();
        for (int i = 0; i < texs.Length; i++)
        {
            DestroyImmediate(texs[i]);
        }

        // TEST
        // Material mat = new Material(RenderSettings.skybox);
        // mat.SetTexture("_Tex", source);
        // RenderSettings.skybox = mat;
        // END TEXT

        // We change the Cubemap of the Skybox
        RenderSettings.skybox.SetTexture("_Tex", cube);
    }

    /// <summary>
    /// Generates a Texture that represents the given face for the cubemap.
    /// </summary>
    /// <param name="resolution">The targetresolution in pixels</param>
    /// <param name="face">The target face</param>
    /// <returns></returns>
    private Color[] CreateCubemapTexture(int resolution, CubemapFace face)
    {
        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);

        Vector3 texelX_Step = (faces[(int)face][1] - faces[(int)face][0]) / resolution;
        Vector3 texelY_Step = (faces[(int)face][3] - faces[(int)face][2]) / resolution;

        float texelSize = 1.0f / resolution;
        float texelIndex = 0.0f;

        //Create textured face
        Color[] cols = new Color[resolution];
        for (int y = 0; y < resolution; y++)
        {
            Vector3 texelX = faces[(int)face][0];
            Vector3 texelY = faces[(int)face][2];
            for (int x = 0; x < resolution; x++)
            {
                cols[x] = Project(Vector3.Lerp(texelX, texelY, texelIndex).normalized);
                texelX += texelX_Step;
                texelY += texelY_Step;
            }
            texture.SetPixels(0, y, resolution, 1, cols);
            texelIndex += texelSize;
        }
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        Color[] colors = texture.GetPixels();
        //DestroyImmediate(texture);

        return colors;
    }

    /// <summary>
    /// Projects a directional vector to the texture using spherical mapping
    /// </summary>
    /// <param name="direction">The direction in which you view</param>
    /// <returns></returns>
    private Color Project(Vector3 direction)
    {
        float theta = Mathf.Atan2(direction.z, direction.x) + Mathf.PI / 180.0f;
        float phi = Mathf.Acos(direction.y);

        int texelX = (int)(((theta / Mathf.PI) * 0.5f + 0.5f) * source.width);
        if (texelX < 0) texelX = 0;
        if (texelX >= source.width) texelX = source.width - 1;
        int texelY = (int)((phi / Mathf.PI) * source.height);
        if (texelY < 0) texelY = 0;
        if (texelY >= source.height) texelY = source.height - 1;

        return source.GetPixel(texelX, source.height - texelY - 1);
    }
}

