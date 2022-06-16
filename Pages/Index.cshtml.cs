using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace aspnetcore_example.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private string GatewayPageURL;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public RedirectResult OnPost()
    {
        var baseUrl = Request.Scheme + "://" + this.Request.Host;
        var PostData = new MultipartFormDataContent();

        PostData.Add(new StringContent(Request.Form["amount"]), "total_amount");
        PostData.Add(new StringContent("BDT"), "currency");

        // System Information
        PostData.Add(new StringContent("testbox"), "store_id");
        PostData.Add(new StringContent("qwerty"), "store_passwd");
        PostData.Add(new StringContent("test_tran_001"), "tran_id");
        PostData.Add(new StringContent(baseUrl + "/Success"), "success_url");
        PostData.Add(new StringContent(baseUrl + "/Fail"), "fail_url"); // "Fail.aspx" page needs to be created
        PostData.Add(new StringContent(baseUrl + "/Cancel"), "cancel_url"); // "Cancel.aspx" page needs to be created
        PostData.Add(new StringContent(baseUrl + "/Ipn"), "ipn_url"); // Backend IPN needs to be implemented

        // Customer Information
        PostData.Add(new StringContent("Customer Name"), "cus_name");
        PostData.Add(new StringContent("abc@example.com"), "cus_email");
        PostData.Add(new StringContent("Address"), "cus_add1");
        PostData.Add(new StringContent("Dhaka"), "cus_city");
        PostData.Add(new StringContent("1000"), "cus_postcode");
        PostData.Add(new StringContent("Bangladesh"), "cus_country");
        PostData.Add(new StringContent("0111111111"), "cus_phone");

        // Misc Information
        PostData.Add(new StringContent("NO"), "shipping_method");
        PostData.Add(new StringContent("NO"), "num_of_item");
        PostData.Add(new StringContent("Shoes"), "product_category");
        PostData.Add(new StringContent("Red Shoe"), "product_name");
        PostData.Add(new StringContent("physical-goods"), "product_profile");
        PostData.Add(new StringContent("0"), "emi_option");

        string ApiUrl = "https://sandbox.sslcommerz.com/gwprocess/v4/api.php";

        var c = new HttpClient();
        var wr = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
        {
            Content = PostData
        };

        var r = c.Send(wr);
        var response = r.Content.ReadAsStream();
       
       SSLCommerzInitRes? res = 
                JsonSerializer.Deserialize<SSLCommerzInitRes>(response);

        return Redirect(res?.GatewayPageURL);

    }
}

public class SSLCommerzInitRes
{
    public string status { get; set; }
    public string failedreason { get; set; }
    public string sessionkey { get; set; }
    public Gw gw { get; set; }
    public string redirectGatewayURL { get; set; }
    public string redirectGatewayURLFailed { get; set; }
    public string GatewayPageURL { get; set; }
    public string storeBanner { get; set; }
    public string storeLogo { get; set; }
    public List<Desc> desc { get; set; }
    public string is_direct_pay_enable { get; set; }
}

public class Gw
{
    public string visa { get; set; }
    public string master { get; set; }
    public string amex { get; set; }
    public string othercards { get; set; }
    public string internetbanking { get; set; }
    public string mobilebanking { get; set; }
}

public class Desc
{
    public string name { get; set; }
    public string type { get; set; }
    public string logo { get; set; }
    public string gw { get; set; }
}