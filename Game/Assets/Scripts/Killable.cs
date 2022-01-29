using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Killable : MonoBehaviour
{
    [SerializeField]
    private ActorConfig config;
    private float currentHp;
    private List<Material> materials;
    private Dictionary<Material, Color> defaultColors = new Dictionary<Material, Color>();

    // Start is called before the first frame update
    void Start()
    {
        currentHp = config.MaxHP;

        materials = new List<Material>(GetComponentsInChildren<SkinnedMeshRenderer>().SelectMany(skin => skin.materials.ToList()));

        foreach (Material m in materials)
        {
            defaultColors.Add(m, m.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage(float amount)
    {
        currentHp -= amount;
        materials.ForEach(x => x.color = Color.Lerp(Color.red, defaultColors[x], 0.5f));
        StartCoroutine(SetDefaultColors());
    }

    private IEnumerator SetDefaultColors()
    {
        for (float i = 0; i <= 1; i += 0.1f)
        {
            foreach (Material m in materials)
            {
                Color startColor = Color.Lerp(Color.red, defaultColors[m], 0.5f);
                m.color = Color.Lerp(startColor, defaultColors[m], i);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
