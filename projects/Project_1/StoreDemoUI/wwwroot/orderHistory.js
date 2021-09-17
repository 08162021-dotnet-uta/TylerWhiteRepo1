function homePage() {
  location.href = "index.html";
  sessionStorage.user = "";
}

function GetStoreListForHistory() {
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
  var select = document.createElement("select");
  select.name = "stores";
  select.id = "stores"
  form.appendChild(select);
  for (const val of store) {
    var option = document.createElement("option");
    option.value = val;
    option.text = val.charAt(0).toUpperCase() + val.slice(1);
    select.appendChild(option);
  }
  select.onchange = async function () {
    orders = document.getElementById("orders");
    orders.innerHTML = "";
    let storeName = select.value;
    console.log(storeName);
    var storeHistory = await fetch(`store/orders/${storeName}`)
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


  };
  document.getElementById("listofstores").appendChild(select);
}




GetStoreListForHistory();

