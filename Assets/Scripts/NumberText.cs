using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberText : MonoBehaviour
{
    private int value;
    private Text valueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetValue()
    {
        return this.value;
    }

    public void SetValue(int value)
    {
        this.value = value;
        this.valueText.text = value.ToString();
    }

    public static NumberText operator ++(NumberText a)
    {
        a.SetValue(a.value + 1);
        return a;
    }

    public static bool operator ==(NumberText a, int b)
    {
        return a.value == b;
    }

    public static bool operator !=(NumberText a, int b)
    {
        return a.value != b;
    }

    public static bool operator ==(NumberText a, NumberText b)
    {
        return a.value == b.value;
    }

    public static bool operator !=(NumberText a, NumberText b)
    {
        return a.value != b.value;
    }

    public static bool operator <(NumberText a, NumberText b)
    {
        return a.value < b.value;
    }

    public static bool operator >(NumberText a, NumberText b)
    {
        return a.value > b.value;
    }

    public static bool operator <=(NumberText a, NumberText b)
    {
        return a.value <= b.value;
    }

    public static bool operator >=(NumberText a, NumberText b)
    {
        return a.value >= b.value;
    }

    public override bool Equals(object obj)
    {
        return obj is NumberText value &&
               base.Equals(obj) &&
               this.value == value.value;
    }

    public override int GetHashCode()
    {
        int hashCode = 1091060534;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + this.value.GetHashCode();
        return hashCode;
    }

    public NumberText Create(int initialValue)
    {
        this.valueText = this.transform.Find("Value").GetComponent<Text>();
        this.SetValue(initialValue);

        return this;
    }
}
