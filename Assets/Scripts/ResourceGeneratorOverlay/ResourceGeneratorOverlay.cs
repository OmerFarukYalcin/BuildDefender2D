using TMPro;
using UnityEngine;

namespace BuilderDefender
{
    public class ResourceGeneratorOverlay : MonoBehaviour
    {
        [SerializeField] ResourceGenerator resourceGenerator;

        private Transform barTransform;

        private void Start()
        {
            ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData;

            barTransform = transform.Find("bar");

            transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;

            transform.Find("text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        }

        private void Update()
        {
            barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalize(), 1, 1);
        }
    }
}
