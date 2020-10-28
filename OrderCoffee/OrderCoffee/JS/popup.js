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
            '     Category: <input class="w2ui-input" style="margin-bottom: 5px" id="category" ><br>' +
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
            '     Category: <input class="w2ui-input" style="margin-bottom: 5px" id="category" ><br>' +
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