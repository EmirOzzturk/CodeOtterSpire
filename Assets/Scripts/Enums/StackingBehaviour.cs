
public enum StackingBehaviour
{
    Additive, // Gelen etki toplanır. Azalma doğal olarak olmaz.
    Counter, // Bir koşul gerçekleşince azalır.
    Duration, // Tur geçince azalır.
    None // Azalmaz. Yenisi eklenmeye çalışırsa yeni eyki yapmaz.
}
