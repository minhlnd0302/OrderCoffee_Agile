function addProductToListCart(item) {
    debugger
    var cartList = new Array();
    try { 
        cartList = JSON.parse(localStorage.getItem('cartList'));
        if (cartList == null) {
            cartList = new Array();
        }
    }
    catch{
        localStorage.setItem('cartList', JSON.stringify([]));
    }
    
    console.log(cartList);
    console.log(typeof cartList)

    // get product current
    {
        var listProduct = JSON.parse(sessionStorage.getItem('listProduct'));
        var idProduct = item.id; 

        var tmp = listProduct.filter(item =>
            item.Id == idProduct
        )
    } 
    cartList.push(tmp[0])
    console.log(cartList)


    localStorage.setItem('cartList', JSON.stringify(cartList));

    debugger
    window.location = "/Products/Index";

    console.log(idProduct)
}