using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Ability 1")]
    public Image abilityImage1;
    public Text abilityText1;
    public KeyCode ability1Key;
    public float ability1CoolDown = 5;

    public Canvas ability1Canvas;
    public Image ability1Skillshot;

    [Header("Ability 2")]
    public Image abilityImage2;
    public Text abilityText2;
    public KeyCode ability2Key;
    public float ability2CoolDown = 7;

    public Canvas ability2Canvas;
    public Image ability2RangeIndicator;
    public float maxAbility2Distance = 7;


    [Header("Ability 3")]
    public Image abilityImage3;
    public Text abilityText3;
    public KeyCode ability3Key;
    public float ability3CoolDown = 10;

    public Canvas ability3Canvas;
    public Image ability3Cone;


    private bool isAbility1CoolDown = false;
    private bool isAbility2CoolDown = false;
    private bool isAbility3CoolDown = false;

    private float currentAbility1CoolDown;
    private float currentAbility2CoolDown;
    private float currentAbility3CoolDown;

    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;

    private void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        abilityText1.text = "";
        abilityText2.text = "";
        abilityText3.text = "";

        ability1Skillshot.enabled = false;
        ability2RangeIndicator.enabled = false;
        ability3Cone.enabled = false;

        ability1Canvas.enabled = false;
        ability2Canvas.enabled = false;
        ability3Canvas.enabled = false;
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ability1Input();
        Ability2Input();
        Ability3Input();

        AbilityCoolDown(ref currentAbility1CoolDown, ability1CoolDown, ref isAbility1CoolDown, abilityImage1, abilityText1);
        AbilityCoolDown(ref currentAbility2CoolDown, ability2CoolDown, ref isAbility2CoolDown, abilityImage2, abilityText2);
        AbilityCoolDown(ref currentAbility3CoolDown, ability3CoolDown, ref isAbility3CoolDown, abilityImage3, abilityText3);

        Ability1Canvas();
        Ability2Canvas();
        Ability3Canvas();
    }

    private void Ability1Canvas()
    {
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab1Canvas = Quaternion.LookRotation(position - transform.position);

            ab1Canvas.eulerAngles = new Vector3(0, ab1Canvas.eulerAngles.y, ab1Canvas.eulerAngles.z);

            ability1Canvas.transform.rotation = Quaternion.Lerp(ab1Canvas, ability1Canvas.transform.rotation, 0);

        }
    }

    private void Ability2Canvas()
    {
        int layerMask = ~LayerMask.GetMask("Player");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                position = hit.point;
            }

        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Distance);

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);
    }

    private void Ability3Canvas()
    {
        if (ability3Cone.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab3Canvas = Quaternion.LookRotation(position - transform.position);

            ab3Canvas.eulerAngles = new Vector3(0, ab3Canvas.eulerAngles.y, ab3Canvas.eulerAngles.z);

            ability3Canvas.transform.rotation = Quaternion.Lerp(ab3Canvas, ability3Canvas.transform.rotation, 0);

        }
    }


    private void Ability1Input()
    {
        if (Input.GetKeyDown(ability1Key) && !isAbility1CoolDown)
        {


            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;
            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;

            Cursor.visible = true;

        }

        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility1CoolDown = true;
            currentAbility1CoolDown = ability1CoolDown;

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
        }
    }
    private void Ability2Input()
    {
        if (Input.GetKeyDown(ability2Key) && !isAbility2CoolDown)
        {


            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;

            Cursor.visible = false;

        }
        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility2CoolDown = true;
            currentAbility2CoolDown = ability2CoolDown;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }
    private void Ability3Input()
    {
        if (Input.GetKeyDown(ability3Key) && !isAbility3CoolDown)
        {


            ability3Canvas.enabled = true;
            ability3Cone.enabled = true;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;
            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;

            Cursor.visible = true;

        }

        if (ability3Cone.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility3CoolDown = true;
            currentAbility3CoolDown = ability3CoolDown;

            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;
        }
    }

    private void AbilityCoolDown(ref float currentCoolDown, float maxCoolDown, ref bool isCoolDown, Image skillImage, Text skillText)
    {
        if (isCoolDown)
        {
            currentCoolDown -= Time.deltaTime;
            if (currentCoolDown <= 0)
            {
                isCoolDown = false;
                currentCoolDown = 0;

                if (skillImage != null)
                {
                    skillImage.fillAmount = 0;
                }
                if (skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                if (skillImage != null)
                {
                    skillImage.fillAmount = currentCoolDown / maxCoolDown;
                }
                if (skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCoolDown).ToString();
                }
            }


        }
    }
}
