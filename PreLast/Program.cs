using System.Globalization;
var  Words= File.ReadAllLines("C:\\Users\\Давід\\RiderProjects\\PreLast\\PreLast\\ukraine_poi.csv");
Console.WriteLine(CalculateDistance( 50.44075, 30.55263,  48.89283,  22.40971));
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

        if (distance/100<=double.Parse(radius, CultureInfo.InvariantCulture))
        {
          
            Places.Add(Words[i]);
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



