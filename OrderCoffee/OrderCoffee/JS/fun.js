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

    var tonTai = 0; 
    for (var index in cartList) {
        if (cartList[index].Id == idProduct) {
            cartList[index].SoLuong += 1;
            tonTai = 1;
        }
    }

    if (tonTai == 0) {
        tmp[0].SoLuong = 1;
        cartList.push(tmp[0])
    }
    

    console.log(cartList)


    localStorage.setItem('cartList', JSON.stringify(cartList)); 

    debugger
    window.location = "/Products/Index";

    console.log(idProduct)
}

function setWatchingProductId(item) {
    var tmp = item.id;
    sessionStorage.setItem('id', tmp);
    debugger
    tmp = parseInt(tmp[5]) * 100 + parseInt(tmp[6]) * 10 + parseInt(tmp[7]);  
    sessionStorage.setItem('currentProductId', tmp - 1) 

    setInfoCurrentProductModal();
}

function setInfoCurrentProductModal() {
    var index = parseInt(sessionStorage.getItem('currentProductId'));
    var list = JSON.parse(sessionStorage.getItem('listProduct'));
    var pro = list[index];

    $('#img1').attr("src", "../" + pro.Image);
    $('#img2').attr("src", "../" + pro.Image);
    $('#img3').attr("src", "../" + pro.Image);
    $('#img4').attr("src", "../" + pro.Image);
    $('#img5').attr("src", "../" + pro.Image);
    $('#img6').attr("src", "../" + pro.Image);
    $('#img7').attr("src", "../" + pro.Image);
    $('#img8').attr("src", "../" + pro.Image);

    $('#productName').text(pro.Name);
    $('#price').text(pro.Price); 
    return pro;
}

function addToCart() {
    debugger
    var idProduct = sessionStorage.getItem('id')
    var sl = $('#slMua').val();
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

    // get product current
    {
        var listProduct = JSON.parse(sessionStorage.getItem('listProduct')); 

        var tmp = listProduct.filter(item =>
            item.Id == idProduct
        )
    }

    var tonTai = 0;
    for (var index in cartList) {
        if (cartList[index].Id == idProduct) {
            cartList[index].SoLuong += sl;
            tonTai = 1;
        }
    }

    if (tonTai == 0) {
        tmp[0].SoLuong = sl; 
        cartList.push(tmp[0])
    }
     
    console.log(cartList)
     
    localStorage.setItem('cartList', JSON.stringify(cartList)); 
   
    window.location = "/Products/Index";

    console.log(idProduct)
}

function updateCart(idProduct) {
    var listProduct = JSON.parse(sessionStorage.getItem('listProduct'));
    var count = 0;
    for (var item in listProduct) {
        if (item.id == idProduct) {
            console.log(item.id + " " + idProduct)
            console.log(item.SoLuong)
            item.SoLuong -= 1;
            count = item.SoLuong;
        }
    }
    console.log(count)

}     