using System.Text.Json;
using System.Text.Json.Serialization;

var json = File.ReadAllText("autoparts_orders.json");
var list = JsonSerializer.Deserialize<Rootobject>(json); 

Console.WriteLine("Fájlok sikeresen beolvasva!");
//1
var customers = list.orders.GroupBy(x => x.customer_id, (c, x) => new { Customer = c, Count = x.Count() });
var costumerM = customers.FirstOrDefault(x => x.Count == customers.Max(y => y.Count));

Console.WriteLine($"A legtöbbet vásárolt ügyfelünk: {list.customers.FirstOrDefault(x => x.customer_id == costumerM.Customer).name}\n");

//2
var allOrderdItems = new List<Item>();
list.orders.ForEach(x => x.items.ForEach(y => allOrderdItems.Add(y)));
var itemsOrderdInGroup = allOrderdItems.GroupBy(x => x.part_id, y => y.quantity, (x,y) => new {partId = x, Count = y.Sum()});
var mostSoldPartId = itemsOrderdInGroup.FirstOrDefault(x=> x.Count == itemsOrderdInGroup.Max(y => y.Count));
var mostSoldPart = list.parts.FirstOrDefault(x => x.part_id == mostSoldPartId.partId);
Console.WriteLine($"{mostSoldPart.name} ből fogyott a legtöbb!");


//3
var paymentMethods = list.orders.GroupBy(x => x.payment_method, y => y.total, (x, y) => new { Method = x, Count = y.Count(), Sum = y.Sum() });

foreach (var item in paymentMethods)
{
    Console.WriteLine($"{item.Method} -dal {item.Count} szor vásároltak ami összesen: {item.Sum}");
}
Console.WriteLine("");

//4
var deliveryMethods = list.orders.GroupBy(x => x.shipping_method, y => y.total, (x, y) => new { Method = x, Count = y.Count(), Sum = y.Sum() });

foreach (var item in deliveryMethods)
{
    Console.WriteLine($"{item.Method} -dal {item.Count} szor vásároltak ami összesen: {item.Sum}");
}



public class Rootobject
{
    [JsonPropertyName("export_date")]
    public DateTime ExportDate { get; set; }
    public string shop_name { get; set; }
    public List<Customer> customers { get; set; }
    public List<Part> parts { get; set; }
    public List<Order> orders { get; set; }
    public Summary summary { get; set; }
}

public class Summary
{
    public int total_orders { get; set; }
    public int total_customers { get; set; }
    public int total_part_types { get; set; }
    public int total_revenue { get; set; }
}

public class Customer
{
    public int customer_id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string address { get; set; }
}

public class Part
{
    public int part_id { get; set; }
    public string name { get; set; }
    public string manufacturer { get; set; }
    public string sku { get; set; }
    public string category { get; set; }
    public int unit_price { get; set; }
    public int in_stock { get; set; }
}

public class Order
{
    public int order_id { get; set; }
    public int customer_id { get; set; }
    public DateTime order_date { get; set; }
    public string status { get; set; }
    public string shipping_method { get; set; }
    public string payment_method { get; set; }
    public string notes { get; set; }
    public List<Item> items { get; set; }
    public int subtotal { get; set; }
    public int shipping_cost { get; set; }
    public int total { get; set; }
}

public class Item
{
    public int part_id { get; set; }
    public int quantity { get; set; }
    public int unit_price { get; set; }
    public int total { get; set; }
}
