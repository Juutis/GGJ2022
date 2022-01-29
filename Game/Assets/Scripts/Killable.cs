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

    private TargetEntity host;

    private Ragdollizer ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
        currentHp = config.MaxHP;

        materials = new List<Material>(GetComponentsInChildren<SkinnedMeshRenderer>().SelectMany(skin => skin.materials.ToList()));

        foreach (Material m in materials)
        {
            defaultColors.Add(m, m.color);
        }

        ragdoll = GetComponentInChildren<Ragdollizer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Die() {
        TargetEntityManager.main.KillTarget(host);
    }

    public void DealDamage(float amount, Vector3 hitPosition, Vector3 hitDirection, float forceAmount)
    {
        currentHp -= amount;
        materials.ForEach(x => x.color = Color.Lerp(Color.red, defaultColors[x], 0.5f));
        StartCoroutine(SetDefaultColors());

        if (currentHp <= 0)
        {
            ragdoll.Activate(hitPosition, hitDirection, forceAmount);
            Die();
        }
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
