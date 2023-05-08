using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class UIComboManager : MonoBehaviour
{
    public delegate void OnChangeSkillUICallback(UICooltimeHolder.ESkillType _type);
    private OnChangeSkillUICallback onChangeSkillCallback = null;

    [SerializeField]
    private RectTransform group = null;

    private TMP_InputField input = null;
    private TextMeshProUGUI combo = null; // �޺� ���� ��� �ؽ�Ʈ

    private int singularity = 100;

    // for Test
    private int testCombo = 0;


    private void Awake()
    {
        if (group == null) return;

        ActiveComboUI(false);

        TextMeshProUGUI[] texts = group.GetComponentsInChildren<TextMeshProUGUI>(); // �׷� ���� �ִ� Text�� ������
        input = group.GetComponentInChildren<TMP_InputField>(); // �޺� ���� ����� ����

        if (texts.Length <= 0) return;
        
        combo = texts[1]; // �޺��� ������ �����ϱ� ���� ������
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            testCombo += 10;
            SetComboText(testCombo);
        }
    }


    public void Init(OnChangeSkillUICallback _onChangeSkillCallback)
    {
        onChangeSkillCallback = _onChangeSkillCallback;
    }


    /*
        int _combo | �޺�(Ÿ�� Ƚ��)�� ����
    */
    public void SetComboText(int _combo)
    {
        if (input == null) return;

        string text = string.Empty;
        OnlyDigitString(_combo, out text);

        SetComboText(text);
    }
    /*
        string _combo | �޺�(Ÿ�� Ƚ��)�� ����
    */
    public void SetComboText(string _combo)
    {
        if (input == null) return;

        string text = string.Empty;
        OnlyDigitString(_combo, out text);

        int combo = 0;
        if(int.TryParse(text, out combo))
        {
            // Combo�� ���� Skill UI ����
            if (combo < singularity)
                onChangeSkillCallback?.Invoke(UICooltimeHolder.ESkillType.Skill_1);
            else
                onChangeSkillCallback?.Invoke(UICooltimeHolder.ESkillType.Skill_2);

            // Combo�� 0 �����̸� ComboUI ����
            if (combo <= 0)
                ActiveComboUI(false);
            else
                ActiveComboUI(true);
        }
        ComboFontColor(text);
        input.text = text;
    }

    private void ActiveComboUI(bool _active)
    {
        group.gameObject.SetActive(_active);
    }

    /*
        string _combo | �޺�(Ÿ�� Ƚ��)�� ����
    */
    private void ComboFontColor(string _combo)
    {
        int combo = 0;
        OnlyDigit(_combo, out combo);
        ComboFontColor(combo);
    }

    /*
        �޺��� ���� �޺� ��Ʈ ������ ����
        int _combo | �޺�(Ÿ�� Ƚ��)�� ����
    */
    private void ComboFontColor(int _combo)
    {
        if (_combo >= 0 && _combo < 30)
        {
            //Color topColor = new Color(140f, 140f, 140f, 1f);       // #8C8C8CFF
            //Color bottomColor = new Color(255f, 255f, 255f, 1f);    // #FFFFFFFF
            Color topColor = new Color(0.5490196f, 0.5490196f, 0.5490196f, 1f); // #8C8C8CFF
            Color bottomColor = new Color(1f, 1f, 1f, 1f);                      // #FFFFFFFF
            combo.colorGradient = GetGradient(topColor, bottomColor);
        }
        else if (_combo >= 30 && _combo < 60)
        {
            //Color topColor = new Color(255f, 123f, 0f, 1f);         // #FF7B00FF
            //Color bottomColor = new Color(255f, 248f, 146f, 1f);    // #FFF892FF
            Color topColor = new Color(1f, 0.4823529f, 0f, 1f);                 // #FF7B00FF
            Color bottomColor = new Color(1f, 0.972549f, 0.572549f, 1f);        // #FFF892FF
            combo.colorGradient = GetGradient(topColor, bottomColor);
        }
        else if (_combo >= 60 && _combo < 100)
        {
            //Color topColor = new Color(255f, 0f, 82f, 1f);          // #FF0052FF
            //Color bottomColor = new Color(255f, 157f, 47f, 1f);    // #FF9D2FFF
            Color topColor = new Color(1f, 0f, 0.3215686f, 1f);                 // #FF0052FF
            Color bottomColor = new Color(1f, 0.6156863f, 0.1843137f, 1f);      // #FF9D2FFF
            combo.colorGradient = GetGradient(topColor, bottomColor);
        }
        else if (_combo >= 100)
        {
            //Color topColor = new Color(255f, 6f, 171f, 1f);         // #FF06ABFF
            //Color bottomColor = new Color(100f, 171f, 255f, 1f);    // #64ABFFFF
            Color topColor = new Color(1f, 0.02352941f, 0.6705883f, 1f);        // #FF06ABFF
            Color bottomColor = new Color(0.3921569f, 0.6705883f, 1f, 1f);      // #64ABFFFF
            combo.colorGradient = GetGradient(topColor, bottomColor);
        }
    }

    /*
        ���ڿ� �Ǵ� ���ڿ��� ���ڰ� �ƴ� ���ڸ� ���͸��Ͽ� int������ �����
        int _val | ���ڰ� �ƴ� ���ڸ� ���͸��� ��
        out | int _int | ���͸��� ���� �޴� ����
    */
    private void OnlyDigit(int _val, out int _int)
    {
        string str = Regex.Replace(_val.ToString(), @"\D", "");
        
        if(int.TryParse(str, out _int) == false)
            _int = -1;
    }
    /*
        ���ڿ� �Ǵ� ���ڿ��� ���ڰ� �ƴ� ���ڸ� ���͸��Ͽ� int������ �����
        string _val | ���ڰ� �ƴ� ���ڸ� ���͸��� ��
        out | int _int | ���͸��� ���� ���� ����
    */
    private void OnlyDigit(string _val, out int _int)
    {
        string str = Regex.Replace(_val.ToString(), @"\D", "");

        if(int.TryParse(str, out _int) == false)
            _int = -1;
    }
    /*
        ���ڿ� �Ǵ� ���ڿ��� ���ڰ� �ƴ� ���ڸ� ���͸��Ͽ� string������ �����
        int _val | ���ڰ� �ƴ� ���ڸ� ���͸��� ��
        out | string _str | ���͸��� ���� ���� ����
    */
    private void OnlyDigitString(int _val, out string _str)
    {
        _str = Regex.Replace(_val.ToString(), @"\D", "");
    }
    /*
        ���ڿ� �Ǵ� ���ڿ��� ���ڰ� �ƴ� ���ڸ� ���͸��Ͽ� string������ �����
        string _val | ���ڰ� �ƴ� ���ڸ� ���͸��� ��
        out | string _str | ���͸��� ���� ���� ����
    */
    private void OnlyDigitString(string _val, out string _str)
    {
        _str = Regex.Replace(_val.ToString(), @"\D", "");
    }

    /*
        Color _topColor | ��Ʈ ���κ� ����
        Color _bottomColor | ��Ʈ �Ʒ��κ� ����
        Return | VertexGradient | ���� ������ ��ģ Gradient
    */
    private VertexGradient GetGradient(Color _topColor, Color _bottomColor)
    {
        return new VertexGradient(_topColor, _topColor, _bottomColor, _bottomColor);
    }
}
