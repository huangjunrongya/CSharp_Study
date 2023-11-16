namespace study;

/// <summary>
/// 事件中心学习。
/// </summary>
public class EventCenter
{
    /***
     * 订单事件中心
     * 委托：定义事件中心事件处理方法的签名
     * 订单：创建订单，发布事件。
     * 支付：订阅事件，支付完成发布事件
     * 发货：订阅事件
     * 
     */
    
    //委托：用于处理订单事件，当订阅该事件的时候，给出一个符合规则的监听器，也就是方法，也就是处理事件的处理器（方法）。这边要求无返回值并且用Order作为形参。
    public delegate void OrderEventHandler(Order data);
    //集合：用于存储订单事件，一个事件名就是一个事件，可以有很多委托（处理方法）
    public static Dictionary<string, OrderEventHandler> orderEvents=new Dictionary<string, OrderEventHandler>();
    
    //订阅事件
    public static void Subscription(string eventName,OrderEventHandler handler)
    {
        if (!orderEvents.ContainsKey(eventName))
        {
            orderEvents[eventName] = handler;
        }

        else
        {
            //查看委托的调用列表
            // 检查是否已经订阅了同样的处理器
            if (!orderEvents[eventName].GetInvocationList().Contains(handler))
            {
                orderEvents[eventName] += handler;
            }
        }
    }
    //发布事件
    public static void  Publish(string eventName, Order order)
    {
        if ( orderEvents.ContainsKey(eventName))
        {
            //调用委托，执行与委托关联的所有方法
            orderEvents[eventName].Invoke(order);
        }
    }

}
//订单系统
    public class OrderSystem
    {
        //创建订单
        public  void CreateOrder(Order order)
        {
            Console.WriteLine($"订单号：{order.orderId}创建成功");
            Console.WriteLine("开始发布订单事件");
            EventCenter.Publish("order",order);
        }
    }
    
    //支付系统
    public class Pay
    {
        public Pay()
        {
            //订阅订单事件
            EventCenter.Subscription("order",PayMethod);
        }

        //处理订单事件
        public void PayMethod(Order order)
        {
            Console.WriteLine("接收到订单事件的发布");
            Console.WriteLine($"处理订单 {order.orderId} 的支付。请输入 'true' 以确认支付成功，或输入其他任意值以表示支付失败。");
            string input = Console.ReadLine();
            bool state=false;
            if (input!=null)
            {
                state = input.Equals("true", StringComparison.OrdinalIgnoreCase);

            }

            // 如果支付成功，则发布事件
            if (state)
            {
                Console.WriteLine($"订单 {order.orderId} 支付成功。");
                Console.WriteLine("发布支付事件");
                //发布支付事件
                EventCenter.Publish("paymentCompleted", order); 
            }
            else
            {
                Console.WriteLine($"订单 {order.orderId} 支付失败。");
            }
        }
    }

public class User
{
    public static void Main()
    {
        //订阅支付事件
        EventCenter.Subscription("paymentCompleted",Deliver);
        //初始化
        OrderSystem orderSystem = new OrderSystem();
        Pay pay = new Pay();
        Random random = new Random();

        while (true)
        {
            //创建订单
            string id = random.Next(1000).ToString();
            Console.WriteLine("=================================");
            Console.WriteLine("用户开始创建订单");
            orderSystem.CreateOrder(new Order(id,"iphone15ProMax,ipad pro"));
        }

       
        
        
    }
    //发货
    public static void Deliver(Order order)
    {
        Console.WriteLine("接收到支付成功事件，开始发通知");
        //通知
        Console.WriteLine($"订单：{order.orderId}已发货");
    }

}


//订单类，封装订单的参数
public class Order
{
    //订单id
    public string orderId { get; set; }
    //订单商品
    public string orderProduct { get; set; }

    public Order(string orderId,string orderProduct)
    {
        this.orderId = orderId;
        this.orderProduct = orderProduct;
    }
}

