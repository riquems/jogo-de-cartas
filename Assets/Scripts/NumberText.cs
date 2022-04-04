using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script NumberText, representa um texto no editor que tem as caracter�sticas
/// de um n�mero, utilizado para facilitar o trabalho de convers�o entre os dois mundos
/// </summary>
public class NumberText : MonoBehaviour
{
    private int value; // Valor do n�mero
    private Text valueText; // Componente de texto do gameObject

    /*
     * M�todo para ser utilizado na cria��o de um objeto com esse componente
     * como se fosse um construtor
     */
    public NumberText Create(int initialValue)
    {
        this.valueText = this.transform.Find("Value").GetComponent<Text>();
        this.SetValue(initialValue);

        return this;
    }

    /*
     * Pega o valor
     */
    public int GetValue()
    {
        return this.value;
    }

    /*
     * Atribui um valor
     */
    public void SetValue(int value)
    {
        this.value = value;
        this.valueText.text = value.ToString();
    }

    /*
     * Defini��o do operador ++
     */
    public static NumberText operator ++(NumberText a)
    {
        a.SetValue(a.value + 1);
        return a;
    }

    /*
     * Defini��o do operador == para inteiros
     */
    public static bool operator ==(NumberText a, int b)
    {
        return a.value == b;
    }

    /*
     * Defini��o do operador != para inteiros
     */
    public static bool operator !=(NumberText a, int b)
    {
        return a.value != b;
    }

    /*
     * Defini��o do operador ==
     */
    public static bool operator ==(NumberText a, NumberText b)
    {
        return a.value == b.value;
    }

    /*
     * Defini��o do operador !=
     */
    public static bool operator !=(NumberText a, NumberText b)
    {
        return a.value != b.value;
    }

    /*
     * Defini��o do operador <
     */
    public static bool operator <(NumberText a, NumberText b)
    {
        return a.value < b.value;
    }

    /*
     * Defini��o do operador >
     */
    public static bool operator >(NumberText a, NumberText b)
    {
        return a.value > b.value;
    }

    /*
     * Defini��o do operador <=
     */
    public static bool operator <=(NumberText a, NumberText b)
    {
        return a.value <= b.value;
    }

    /*
     * Defini��o do operador >=
     */
    public static bool operator >=(NumberText a, NumberText b)
    {
        return a.value >= b.value;
    }

    /*
     * Defini��o da fun��o de equalidade 
     */
    public override bool Equals(object obj)
    {
        return obj is NumberText value &&
               base.Equals(obj) &&
               this.value == value.value;
    }

    /*
     * Defini��o da opera��o de hashCode
     */
    public override int GetHashCode()
    {
        int hashCode = 1091060534;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + this.value.GetHashCode();
        return hashCode;
    }
}
