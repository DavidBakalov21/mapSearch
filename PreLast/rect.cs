namespace PreLast;



public class Rect
{
    public Point BottomLeft;
    public Point TopRight;
    public Rect(Point bottomLeft, Point topRight)
    {

        this.BottomLeft = bottomLeft;
        this.TopRight = topRight;
    }

    public bool Intersect(Rect Another)
    {
        var Intersection = false;
        if ((this.TopRight.latitude>Another.BottomLeft.latitude || Another.TopRight.latitude>this.BottomLeft.latitude) && (this.TopRight.longitude>Another.BottomLeft.longitude || Another.TopRight.longitude>this.BottomLeft.longitude))
        {
            Intersection = true;
        }
        return Intersection;
    }

}