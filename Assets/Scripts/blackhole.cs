using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class TilemapExplosion : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject fragmentPrefab;

    [Header("Explosion")]
    public float explosionForce = 5f;

    void Update()
    {
        // Tekan SPACE untuk test
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Explode();
        }
    }

    public void Explode()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);

            if (tile != null)
            {
                Vector3 worldPos =
                    tilemap.GetCellCenterWorld(pos);

                // Spawn pecahan
                GameObject frag =
                    Instantiate(
                        fragmentPrefab,
                        worldPos,
                        Quaternion.identity
                    );

                // Ambil sprite tile asli
                Sprite tileSprite =
                    tilemap.GetSprite(pos);

                // Pasang sprite ke fragment
                frag.GetComponent<SpriteRenderer>().sprite =
                    tileSprite;

                Rigidbody2D rb =
                    frag.GetComponent<Rigidbody2D>();

                // Arah random
                Vector2 dir =
                    Random.insideUnitCircle.normalized;

                // Lempar fragment
                rb.AddForce(
                    dir * explosionForce,
                    ForceMode2D.Impulse
                );

                // Rotasi random
                rb.AddTorque(
                    Random.Range(-200f, 200f)
                );

                // Hapus tile asli
                tilemap.SetTile(pos, null);
            }
        }
    }
}