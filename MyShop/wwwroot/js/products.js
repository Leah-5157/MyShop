sessionStorage.setItem("categoryIds", JSON.stringify([]));
sessionStorage.setItem("orderItems", JSON.parse(sessionStorage.getItem("orderItems"))?.length > 0 ? JSON.stringify(JSON.parse(sessionStorage.getItem("orderItems"))): JSON.stringify([]));

addEventListener("load", () => {
    filterProducts()
        getCategory()
    let items = JSON.parse(sessionStorage.getItem("orderItems"))
    document.getElementById("ItemsCountText").textContent = items.length

})


const GetDataFromDocumentForFilter= () => {
    const desc = document.querySelector('#nameSearch')?.value;
    const minPrice = parseInt( document.querySelector('#minPrice')?.value);
    const maxPrice = parseInt(document.querySelector('#maxPrice')?.value);
    const categoryIds = JSON.parse(sessionStorage.getItem("categoryIds"));
    return ({ desc, minPrice, maxPrice, categoryIds })
}


const filterProducts = async () => {
    
    const obj = GetDataFromDocumentForFilter()
    let url = '../api/Product';
    if (obj.desc || obj.minPrice || obj.maxPrice || obj.categoryIds.length!=0)
        url += '?'
    if (obj.desc)
        url += `&desc=${obj.desc}`
    if (obj.minPrice)
        url += `&minPrice=${obj.minPrice}`
    if (obj.maxPrice)
        url += `&maxPrice=${obj.maxPrice}`
    if (obj.categoryIds.length != 0) { 
        for (let i = 0; i < obj.categoryIds.length; i++) 
        url += `&categoryIds=${obj.categoryIds[i]}`
    }
    try {
         products = await fetch(url, {
            method: "GET",
            headers: {
                'Content-type': 'application/json'
            },
            query: {
                desc: obj.desc,
                minPrice: obj.minPrice,
                maxPrice: obj.maxPrice,
                categoryIds: obj.categoryIds
            }
        });
        const data = await products.json();
        console.log(data)
        if (!products.ok) {
            throw new Error(`HTTP error! status:${products.status}`);
        }
        else
            drawProducts(data)

    } catch (error) {
        alert("try again")
        console.log(error)
    }
}

const drawTemplate = async (product) => {
    let tmp = document.getElementById("temp-card");
    let cloneProduct = tmp.content.cloneNode(true)
    cloneProduct.querySelector("img").src = "../img/" + product.imgURL
    cloneProduct.querySelector("h1").textContent = product.productName
    cloneProduct.querySelector(".price").innerText = product.price
    cloneProduct.querySelector(".description").innerText = product.description
    console.log(document.getElementById("bag"))
    cloneProduct.getElementById("bag").addEventListener('click', () => { addToCart(product) })
    console.log(document.getElementById("PoductList"))
    document.getElementById("PoductList").appendChild(cloneProduct)
}
const drawProducts = async (products) => {
    document.getElementById("PoductList").innerHTML=""
    for (let i = 0; i < products.length; i++) {
        drawTemplate(products[i])
    }
 
}
const addToCart = (product) => {
    let items = JSON.parse(sessionStorage.getItem("orderItems"))
    items.push(product)
    sessionStorage.setItem("orderItems", JSON.stringify(items))
    document.getElementById("ItemsCountText").textContent = items.length
}

const getCategory = async () => {
    try {
        const categories = await fetch('../api/Category', {
            method: "GET",
            headers: {
                'Content-type': 'application/json'
            }
        });
        const data = await categories.json();
        console.log(data)
        if (!categories.ok) {
            throw new Error(`HTTP error! status:${categories.status}`);
        }
        else
            drawcategories(data)

    } catch (error) {
        alert("try again")
        console.log(error)
    }
}
const drawcategories = (category) => {
    for (let i = 0; i < category.length; i++) {
        let tmp = document.getElementById("temp-category");
        let cloneCategory = tmp.content.cloneNode(true)
        cloneCategory.querySelector(".OptionName").textContent = category[i].categoryName
        cloneCategory.querySelector(".opt").addEventListener('change', () => { filterCategory(category[i]) })
        document.getElementById("categoryList").appendChild(cloneCategory)
    }

    const filterCategory = (category) => {
        let categories = JSON.parse(sessionStorage.getItem("categoryIds"))
        let a = categories.indexOf(category.categoryId)
        a == -1 ? categories.push(category.categoryId) : categories.splice(a, 1)
        sessionStorage.setItem("categoryIds", JSON.stringify(categories))
        console.log(categories)
        filterProducts()
    }
}

