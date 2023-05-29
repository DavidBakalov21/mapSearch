using System.Globalization;
using PreLast;


/*
using System.Diagnostics;

var sw = new Stopwatch();
sw.Start();
var  Words= File.ReadAllLines("C:\\Users\\Давід\\RiderProjects\\PreLast\\PreLast\\ukraine_poi.csv");
//Console.WriteLine(CalculateDistance( 50.44075, 30.55263,  48.89283,  22.40971));
var latitude1 = Console.ReadLine();
var longitude1 = Console.ReadLine();
var radius = Console.ReadLine();
var Places = new List<string>();
var t = 4;
for (int i = 0; i < Words.Length; i++)
{
    var split = Words[i].Split(";");
    var latitude = split[0].Replace(",", ".");
    var longitude = split[1].Replace(",", ".");
    if (latitude!="" && longitude!="" && latitude!=null && longitude!=null)
    {
        var distance=CalculateDistance( double.Parse(latitude1, CultureInfo.InvariantCulture), double.Parse(longitude1, CultureInfo.InvariantCulture),  double.Parse(latitude, CultureInfo.InvariantCulture),  double.Parse(longitude, CultureInfo.InvariantCulture));

        if (distance/1000<=double.Parse(radius, CultureInfo.InvariantCulture))
        {
          
            Places.Add(Words[i]+"--distance--"+distance/1000);
        }  
    }
}

foreach (var VARIABLE in Places)
{
    Console.WriteLine(VARIABLE);
}
double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
{

    double rLat =(lat2 - lat1)* (Math.PI / 180);
    double rLon = (lon2 - lon1)* (Math.PI / 180);

    double a = Math.Sin(rLat/2) * Math.Sin(rLat/2) +
        Math.Cos(lat1 * Math.PI/180) * Math.Cos(lat2 * Math.PI/180) *
        Math.Sin(rLon/2) * Math.Sin(rLon/2);
    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    double distance =6371e3 * c;
    return distance;
}

sw.Stop();
Console.WriteLine($"Elapsed time: {sw.Elapsed}");
*/


var words = File.ReadAllLines("C:\\Users\\Давід\\RiderProjects\\PreLast\\PreLast\\test.csv");
var latList = new List<double>();
var longList = new List<double>();
var Points = new List<Point>();
for (int i = 0; i < words.Length; i++)
{
    
    var split = words[i].Split(";");
   
   
    var latitude = split[0].Replace(",", ".");;
    var longitude = split[1].Replace(",", ".");;
    if (latitude != "" && longitude != "" && latitude != null && longitude != null)
    {
        var t = split.Length;
        var name = "";
        while (split[t-1]=="")
        {
            if (split[t-1]!="")
            {
                break;
            }

            t -= 1;
        }
       
            name = split[t-1];
      
        latList.Add(double.Parse(latitude, CultureInfo.InvariantCulture));
        longList.Add(double.Parse(longitude, CultureInfo.InvariantCulture));
        Points.Add(new Point(name, double.Parse(longitude, CultureInfo.InvariantCulture),double.Parse(latitude, CultureInfo.InvariantCulture)));
    }
}

var bottom_leftPoint= new Point("bottom_left", longList.Min(), latList.Min());
var top_rightPoint= new Point("top_right", longList.Max(), latList.Max());
Console.WriteLine(bottom_leftPoint.latitude);
Console.WriteLine(bottom_leftPoint.longitude);
//create a point class(lat, long , type)-Done
    // craete  rect (class point) (min lat min long; max lat max lat)
    //class tree with root(class) (in all classes rect heir)
    
    
    // throw pointlist and radius to func 

foreach (var VARIABLE in Points)
{
    var output = VARIABLE.name+" " + VARIABLE.latitude+" "+ VARIABLE.longitude;
    Console.WriteLine(output);
}
KdTree buildTree(List<Point> pointList)
{
    KdTree kdTree = new KdTree();
    kdTree.BuildTree(pointList);
    return kdTree;
}

var a = buildTree(Points);
var b= buildTree(Points);


class Node
{
    public Point value { get; set; }
    public Node left { get; set; }
    public Node Right { get; set; }

    public Node(Point Value)
    {
        value = Value;
    }
}

class KdTree
{
    public Node Root { get; set; }

    public KdTree()
    {
        Root = null;
    }

    public void BuildTree(List<Point> pointList)
    {
        Root = BuildTreeRecursive(pointList, 0);
    }

    private Node BuildTreeRecursive(List<Point> pointList, int depth)
    {
        if (pointList.Count == 0)
        {
            return null;
        }

        int axis = depth % 2; // Alternate between x-axis (longitude) and y-axis (latitude)
        pointList.Sort((a, b) => axis == 0 ? a.longitude.CompareTo(b.longitude) : a.latitude.CompareTo(b.latitude));
        int medianIndex = pointList.Count / 2;
        Point medianPoint = pointList[medianIndex];

        Node node = new Node(medianPoint);
        node.left = BuildTreeRecursive(pointList.GetRange(0, medianIndex), depth + 1);
        node.Right = BuildTreeRecursive(pointList.GetRange(medianIndex + 1, pointList.Count - medianIndex - 1), depth + 1);

        return node;
    }
}