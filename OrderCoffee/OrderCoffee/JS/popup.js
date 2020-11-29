function popupEditProduct(infoProduct) {
    w2popup.open({
        width: 580,
        height: 350,
        title: 'Edit Product',
        body: '<div class="w2ui-centered" style="line-height: 1.8">' +
            '     Fill in the information to change in the fields below<br/><br/>' +
            '     Product Name:<input class="w2ui-input" style="margin-bottom: 5px" id="productname" ><br>' +
            '     Price : <input class="w2ui-input" style="margin-bottom: 5px" id="price" ><br>' +
            '     Description: <input class="w2ui-input" style="margin-bottom: 5px" id="description" ><br>' +
            '     Category: <select id="category" class="w2ui-input" style="margin-bottom: 5px" name="category"><option value="Coffee Decaf">Coffee Decaf</option><option value="Coffee Moka">Coffee Moka</option><option value="Coffee Culi">Coffee Culi</option><option value="Cherry">Cherry</option><option value="Arabica">Arabica</option><option value="Robusta">Robusta</option></select> <br>' +
            '  </div>',
        buttons: '<button class="w2ui-btn" onclick="saveDataUpdate()">Save</button>' +
            '<button class="w2ui-btn" onclick="w2popup.close()">Cancel</button>'
    });

    debugger
    infoProduct = infoProduct[0];

    $('#productname').val(infoProduct.Name);
    $('#price').val(infoProduct.Price);
    $('#description').val(infoProduct.Description);

    var listCategory = JSON.parse(sessionStorage.getItem('listCategory')); 

    var categoryName = listCategory.filter(item => {
        return item.Id == infoProduct.Id_Category;
    })  
    $('#category').val(categoryName[0].Category_Name);
    //$('#category').autocomplete({ source: getListCategoryName() });

}

function popupEditCoupon(infoProduct) {
    w2popup.open({
        width: 580,
        height: 350,
        title: 'Edit Product',
        body: '<div class="w2ui-centered" style="line-height: 1.8">' +
            '     Precent: <select id="precent">' +
            '        <option value="10">10%</option>' +
            '        <option value="20">20%</option>' +
            '        <option value="30">30%</option>' +
            '        <option value="40">40%</option>' +
            '        </select ><br>' +
            '     Date Start: <input type="date" style="margin-bottom: 5px" id="datestart" ><br>' +
            '     Date End: <input type="date" style="margin-bottom: 5px" id="dateend" ><br>' +
            '  </div>',
        buttons: '<button class="w2ui-btn" onclick="saveDataUpdateCoupon()">Save</button>' +
            '<button class="w2ui-btn" onclick="w2popup.close()">Cancel</button>'
    });

    debugger
    infoProduct = infoProduct[0];

    $('#price').val(infoProduct.precent);
    $('#datestart').val(infoProduct.datestart);
    $('#dateend').val(infoProduct.dateend);

    var listdiscountCode = JSON.parse(sessionStorage.getItem('listdiscount_code'));

    var categoryName = listdiscountCode.filter(item => {
        return item.Id == infoProduct.Id_Category;
    })
    //$('#category').val(categoryName[0].Category_Name);
}

function popupAddProduct() {
    w2popup.open({
        width: 580,
        height: 350,
        title: 'Add Product',
        body: '<div class="w2ui-centered" style="line-height: 1.8">' +
            '     Enter new product information !<br/><br/>' +
            '     Product Name:<input class="w2ui-input" style="margin-bottom: 5px" id="productname" ><br>' +
            '     Price : <input class="w2ui-input" style="margin-bottom: 5px" id="price" ><br>' +
            '     Description: <input class="w2ui-input" style="margin-bottom: 5px" id="description" ><br>' +
            '     Category: <select id="category" class="w2ui-input" style="margin-bottom: 5px" name="category"><option value="Coffee Decaf">Coffee Decaf</option><option value="Coffee Moka">Coffee Moka</option><option value="Coffee Culi">Coffee Culi</option><option value="Cherry">Cherry</option><option value="Arabica">Arabica</option><option value="Robusta">Robusta</option></select> <br>' +
            '  </div>',
        buttons: '<button class="w2ui-btn" onclick="addproduct()">Add</button>' +
            '<button class="w2ui-btn" onclick="w2popup.close()">Cancel</button>'
    }); 
}

function addproduct() {
    var dataNewProduct = getDataUpdate();
    var postUrl = '/Home/addProduct'

    request(postUrl, dataNewProduct).then(res => {
        console.log("OK");
        w2popup.close()
    })
    console.log(dataNewProduct)
}

function showProducts() {
    debugger
    request('/Home/getInfoListProduct', {}).then(res => {
        debugger
        console.log(res.data);
    })
    //var products = @Html.Raw(Json.Encode(ViewBag.listProduct));
    //var listCategory = @Html.Raw(Json.Encode(ViewBag.listCategory));

    sessionStorage.setItem('listCategory', JSON.stringify(listCategory));

    for (var item of products) {
        delete item['Image'];
        item.recid = item.Id;
        delete item['Id'];
    }
    console.log(products)
    console.log(listCategory)
    $('#grid').w2grid({
        name: 'grid',
        show: {
            toolbar: true,
            footer: true
        },
        multiSearch: true,
        searches: [
            { field: 'recid', caption: 'ID ', type: 'text' },
            { field: 'Id_Category', caption: 'ID Category', type: 'text' },
            { field: 'Name', caption: 'Name product', type: 'text' },
            { field: 'Price', caption: 'Price', type: 'int' },
            { field: 'Description', caption: 'Description', type: 'text' },
            { field: 'Quantity', caption: 'Quantity', type: 'int' }
        ],
        columns: [
            { field: 'recid', caption: 'ID', size: '50px', sortable: true, size: '10%' },
            { field: 'Id_Category', caption: 'ID Category', size: '10%', sortable: true },
            { field: 'Name', caption: 'Name product', size: '20%', sortable: true },
            { field: 'Price', caption: 'Price', size: '10%', sortable: true },
            { field: 'Description', caption: 'Description', size: '60%' },
            { field: 'Quantity', caption: 'Quantity', size: '8%' }
        ],
        records: products
    });

    function addProduct() {
        debugger
        popupAddProduct();
    }

    function addImages() {
        var productSelected = w2ui['grid'].getSelection();
        if (productSelected.length > 1 || productSelected.length < 1) {
            alert('Please choice a product !');
            return;
        }

        sessionStorage.setItem("currentProduct", productSelected)


    }

    function editProduct() {
        var productSelected = w2ui['grid'].getSelection();
        if (productSelected.length > 1 || productSelected.length < 1) {
            alert('Please choice a product !');
            return;
        }

        var infoProductSelected = w2ui['grid'].records.filter(item => {
            return item.recid == productSelected;
        })
        debugger
        console.log(infoProductSelected);

        sessionStorage.setItem('idProductCurrent', infoProductSelected[0].recid);

        popupEditProduct(infoProductSelected);
    }

    function removeProduct() {
        var productSelected = w2ui['grid'].getSelection();
        sessionStorage.setItem('idProductCurrent', productSelected);
        showConfirmRemove();
    }
}

// ------------------ Coupon ------------------------- //

function popupAddCode() {
    w2popup.open({
        width: 580,
        height: 350,
        title: 'Add Code',
        body: '<div class="w2ui-centered" style="line-height: 1.8">' +
            '     Enter new coupon information !<br/><br/>' +
            '     Code: <input class="w2ui-input" style="margin-bottom: 5px" id="code" ><br>' +
            '     Precent: <select id="precent">' +
            '        <option value="10">10%</option>' +
            '        <option value="20">20%</option>' +
            '        <option value="30">30%</option>' +
            '        <option value="40">40%</option>' +
            '        </select ><br>' +
            '     Date Start: <input type="date" style="margin-bottom: 5px" id="datestart" ><br>' +
            '     Date End: <input type="date" style="margin-bottom: 5px" id="dateend" ><br>' +
            '  </div>',
        buttons: '<button class="w2ui-btn" onclick="addcoupon()">Add</button>' +
            '<button class="w2ui-btn" onclick="w2popup.close()">Cancel</button>'
    });
}

function Request(postUrl, postData) {
    console.log(postUrl);
    return $.post(postUrl, postData,
        function (res, status) {
            return res;
        }
    )
}

function dataCoupon(code, precent, datestart, dateend) {
    this.Id = sessionStorage.getItem('idCouponCurrent');
    this.Code = code;
    this.Disc_Percent = precent;
    this.Date_Start = datestart;
    this.Date_End = dateend;
}

function addcoupon() {
    var dataNewCoupon = getDataUpdateCoupon();
    var postUrl = '/Home/addCoupon'

    Request(postUrl, dataNewCoupon).then(res => {
        alert("Add coupon complete !!!");
        resetdataCoupon();
        w2popup.close()
    })
    console.log(dataNewCoupon)
}

function resetdataCoupon() {
    Request('/Home/getInfoListCoupon', null).then(res => {
        var discount_code = res;
        
    })
}

function getDataUpdateCoupon(i) {
    debugger
    console.log(i);
    var code = $('#code').val();
    var precent = $('#precent').val();
    var datestart = $('#datestart').val();
    var dateend = $('#dateend').val();

    var updateCouponData = new dataCoupon(code, precent, datestart, dateend);
    debugger
    console.log(code);
    return updateCouponData;
}

function saveDataUpdateCoupon() {
    var dataUpdate = getDataUpdateCoupon();

    var postUrl = '/Home/updateCoupon';

    Request(postUrl, dataUpdate).then(res => {
        if (res.data.status === 'OK') {
            alert('Edit complete !')
        }
        else {
            alert('Error !')
        }
    })

    w2popup.close()
}

function showConfirmRemoveCoupon() {
    debugger
    w2confirm('Are you sure you want to delete this coupon?')
        .yes(function () {
            debugger
            var postUrl = '/Home/removeCoupon';
            var id = sessionStorage.getItem('idProductCurrent');
            request(postUrl, { 'Id': id }).then(res => {
            })
        })
        .no(function () { console.log('NO'); });

    debugger

}

// ----------------------- Coupon ---------------------- //


function showConfirmRemove() {
    debugger
    w2confirm('Are you sure you want to delete this product?')
        .yes(function () {
            debugger
            var postUrl = '/Home/removeProduct';
            var id = sessionStorage.getItem('idProductCurrent');
            request(postUrl, { 'Id': id }).then(res => {
                //if (res.Data.status == 'F') {
                //    alert('Error !')
                //}
                //else { alert('Remove complete !') }
            })
        })
        .no(function () { console.log('NO'); });

    debugger
   
} 
function remove() {
    var postUrl = '/Home/removeProduct';
    var postData = { Id: id, };
    request(postUrl, data).then(res => {
        if (res.Data.status == 'F') {
            alert('Error !')
        }
        else { alert('Remove complete !') }
    })
}

function getFilePath() {
    $('input[type=file]').change(function () {
        var filePath = $('#fileUpload').val();
        return filePath;
    });
} 

function dataProduct(productname, price, description, category, filepath) {
    this.Id = sessionStorage.getItem('idProductCurrent');
    this.Name = productname;
    this.Price = price;
    this.Description = description;
    this.CategoryName = category;
    this.Image = filepath;
    //this.Quantity = 0;
} 

function getDataUpdate(i) {
    debugger
    console.log(i);
    var productname = $('#productname').val();
    var price = $('#price').val();
    var description = $('#description').val();
    var category = $('#category').val();
    var filepath = "";

    var updateProductData = new dataProduct(productname, price, description, category, filepath);
    debugger
    console.log(productname);
    return updateProductData;
}

function saveDataUpdate() {
    var dataUpdate = getDataUpdate();

    var postUrl = '/Home/updateProduct'; 

    request(postUrl, dataUpdate).then(res => {
        if (res.data.status === 'OK') {
            alert('Edit complete !')
        }
        else {
            alert('Error !')
        }
    })

    w2popup.close()
}


function getListCategoryName() {
    var listCategory = JSON.parse(sessionStorage.getItem('listCategory')); 
    listCategory = listCategory.map(item => {
        return item.Category_Name;
    }) 
    return listCategory;
}

function request(postUrl, postData) {
    return $.post(postUrl, postData,
        function (res, status) {
            return res;
        }
    )
}