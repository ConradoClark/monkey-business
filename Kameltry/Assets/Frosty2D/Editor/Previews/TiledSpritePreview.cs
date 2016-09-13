using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPreview(typeof(FrostyTiledSprite))]
public class TiledSpritePreview : ObjectPreview
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        FrostyTiledSprite ts = target as FrostyTiledSprite;
        GUI.DrawTexture(r, ts.sprite.texture);
    }
    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        FrostyTiledSprite ts = target as FrostyTiledSprite;
        if (ts.sprite == null) return;
        float xOffset = r.width / 2 - ts.sprite.bounds.size.x / 2 * ts.numberOfTiles.x;
        float yOffset = r.height / 2 - ts.sprite.bounds.size.y / 2 * ts.numberOfTiles.y;

        for (int i = 0; i < ts.numberOfTiles.x; i++)
        {
            for (int j = 0; j < ts.numberOfTiles.y; j++)
            {
                GUI.DrawTexture(new Rect(r.x + xOffset + i * ts.sprite.bounds.size.x, r.y + yOffset + j * ts.sprite.bounds.size.y, ts.sprite.bounds.size.x, ts.sprite.bounds.size.y), ts.sprite.texture);
            }
        }
    }

    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("Tile Result");
    }
}
