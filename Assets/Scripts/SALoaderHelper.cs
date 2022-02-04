using UnityEngine;

public class SALoaderHelper : MonoBehaviour
{
    public void LoadAtlas(string sprspriteAtlasName)
    {
        SpriteAtlasLoader.Instance.LoadAtlas(sprspriteAtlasName);
    }
}
