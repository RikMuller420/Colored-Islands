public class Unit
{
    public Unit(Island island, Paint paint)
    {
        Island = island;
        Paint = paint;
    }

    public Paint Paint { get; private set; }
    public Island Island { get; private set; }

    public void ActivateOutline()
    {
        //enable outline
    }

    public void DeactivateOutline()
    {
        // disable outline
    }
}
