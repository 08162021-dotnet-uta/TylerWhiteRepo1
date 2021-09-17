const welcomediv = document.querySelector('.welcomediv');
var currentOrderList = [];
var select;

function uuidv4() {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}


if (!sessionStorage.user) {
  location.href = "index.html";
}
else {
  //console.log(sessionStorage.user.fname);
  let user = JSON.parse(sessionStorage.getItem('user'));
  console.log(user);
  welcomediv.innerHTML = `${user.fname}  ${user.lname}`;

  // create an element and give it some inntertext/HTML
  const newDiv = document.createElement("div");
  // and give it some content
  const newContent = document.createTextNode("Hi there and greetings!");
  // add the text node to the newly created div
  newDiv.appendChild(newContent);
  // add the newly created element and its content into the DOM
  const currentDiv = document.getElementsByTagName("script");
  //welcomediv.insertBefore(newDiv, currentDiv[0]);

  GetStoreList();
}

function logOut() {
  location.href = "index.html";
  sessionStorage.user = "";
}

async function pastOrders() {
  orders = document.getElementById("orders");
  document.getElementById("products").innerHTML = "";
  orders.innerHTML = "";
  let user = JSON.parse(sessionStorage.getItem('user'));
  let userId = user["customerId"];
  await fetch(`store/pastOrders/${userId}`)
    .then(res => {
      if (!res.ok) {
        console.log('unable to fetch stores')
        throw new Error(`Network response was not ok (${res.status})`);
      }
      return res.json();
    })
    .then(res => {
      // console.log(res);
      sessionStorage.setItem('storeHistoy', JSON.stringify(res));
      // the below are all valid ways to get the values back from the storage object.
      //console.log(sessionStorage.getItem('storeHistoy'));
      // console.log(sessionStorage.store);
      // console.log(sessionStorage['store']);
      //sessionStorage.clear()

    })
    .catch(err => console.log(`There was an error ${err}`));
  let storeHist = JSON.parse(sessionStorage.storeHistoy);
  console.log(storeHist);
  for (let i = 0; i < storeHist.length; i++) {
    console.log(storeHist[i]);
    var option = document.createElement("ul");
    option.innerHTML += "<li> <Label>Store ID: </Label>  " + storeHist[i]["storeid"] + "</li>   ";
    option.innerHTML += "<li> <Label>Store Name:  </Label>  " + storeHist[i]["storeName"] + "</li>   ";
    option.innerHTML += "<li> <Label>Order ID:  </Label>  " + storeHist[i]["orderid"] + "</li>   ";
    option.innerHTML += "<li> <Label>Customer ID:  </Label>  " + storeHist[i]["customerId"] + "</li>   ";
    option.innerHTML += "<li> <Label>Order Date:  </Label>  " + storeHist[i]["orderDate"] + "</li>   ";
    option.innerHTML += "<li> <Label>Product ID:  </Label>  " + storeHist[i]["productId"] + "</li>   ";
    var div = document.createElement("div");
    div.appendChild(option);
    orders.appendChild(div);
  }
}

function checkOut() {
  let orderID = uuidv4();
  for (const val of currentOrderList) {
    console.log(val);
    console.log(val["productId"]);
    let storeName = select.value;
    let user = JSON.parse(sessionStorage.getItem('user'));
    let userId = user["customerId"];
    let productId = val["productId"];
    console.log(user["customerId"]);
    let storeID = 1;
    if (storeName == "Walmart") storeID = 1;
    else if (storeName == "Safeway") storeID = 2;
    else if (storeName == "Cosco") storeID = 3;
    else if (storeName == "Savemart") storeID = 4;

    fetch(`store/saveOrder/${storeID}/${storeName}/${orderID}/${userId}/${productId}`)
      .then(res => {
        if (!res.ok) {
          console.log('unable to login the user')
          throw new Error(`Network response was not ok (${res.status})`);
        }
        return res.json();
      })
      .then(res => {
        console.log(res);
        sessionStorage.setItem('user', JSON.stringify(res));
        // the below are all valid ways to get the values back from the storage object.
        console.log(sessionStorage.getItem('user'));
        console.log(sessionStorage.user);
        console.log(sessionStorage['user']);
        //sessionStorage.clear()

        location.href = "index.html";
      })
      .catch(err => console.log(`There was an error ${err}`));
  }
  logOut();
}



// offer choices to navigate
//choose a store to shop from
function GetStoreList() {
  var values = fetch(`store/stores`)
    .then(res => {
      if (!res.ok) {
        console.log('unable to fetch stores')
        throw new Error(`Network response was not ok (${res.status})`);
      }
      return res.json();
    })
    .then(res => {
      // console.log(res);
      sessionStorage.setItem('store', JSON.stringify(res));
      // the below are all valid ways to get the values back from the storage object.
      console.log(sessionStorage.getItem('store'));
      // console.log(sessionStorage.store);
      // console.log(sessionStorage['store']);
      //sessionStorage.clear()

    })
    .catch(err => console.log(`There was an error ${err}`));
  let store = JSON.parse(sessionStorage.getItem('store'));
  console.log(store);
  var form = document.createElement("form");
  select = document.createElement("select");
  select.name = "stores";
  select.id = "stores"
  form.appendChild(select);
  for (const val of store) {
    var option = document.createElement("option");
    option.value = val;
    option.text = val.charAt(0).toUpperCase() + val.slice(1);
    select.appendChild(option);
  }

  function addToCart() {
    let currentOrder = document.getElementById("currentOrder");
    let tempProduct = document.createElement("li");
    let value = JSON.parse(this.value);
    currentOrderList.push(value);
    tempProduct.innerHTML = "<Label>" + value["productName"] + " : $" + value["productPrice"] + "</Label>   ";
    let currentPrice = parseFloat(document.getElementById("price").innerHTML);
    currentPrice += parseFloat(value["productPrice"]);
    document.getElementById("price").innerHTML = parseFloat(currentPrice).toFixed(2);
    currentOrder.appendChild(tempProduct);
  };


  select.onchange = function () {
    productsMain = document.getElementById("products");
    document.getElementById("orders").innerHTML = "";
    while (productsMain.firstChild) {
      productsMain.removeChild(productsMain.firstChild);
    }
    var products = fetch(`store/products`)
      .then(res => {
        if (!res.ok) {
          console.log('unable to fetch products')
          throw new Error(`Network response was not ok (${res.status})`);
        }
        return res.json();
      })
      .then(res => {
        //console.log(res);
        sessionStorage.setItem('products', JSON.stringify(res));
        // the below are all valid ways to get the values back from the storage object.
        //console.log(sessionStorage.getItem('products'));
        // console.log(sessionStorage.products);
        //console.log(sessionStorage['products']);
        //sessionStorage.clear()

      })
      .catch(err => console.log(`There was an error ${err}`));
    products = JSON.parse(sessionStorage.products);
    console.log(products);
    for (const val of products) {
      var button = document.createElement("button");
      button.onclick = addToCart;
      button.innerHTML = "Add to Cart";
      button.value = JSON.stringify(val);
      button.style.width = "85px";
      button.style.height = "25x";
      var option = document.createElement("Label");
      option.innerHTML = "<Label>" + val["productName"] + " : $" + val["productPrice"] + "</Label>   ";
      var div = document.createElement("div");
      div.appendChild(option);
      div.appendChild(button);
      productsMain.appendChild(div);
      // option.text = val.charAt(0).toUpperCase() + val.slice(1);
      //productsMain.appendChild(option);
    }
  };

  document.getElementById("listofstores").appendChild(select);
}
// log out