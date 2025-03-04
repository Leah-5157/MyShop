const GetDataFromDocumentForFilter = () => {
    const minPrice = parseInt( document.querySelector("#minPrice").value);
    const maxPrice = parseInt(document.querySelector("#maxPrice").value);
    const categoryIds =JSON.parse(sessionStorage.getItem("categoryIds"));
    const desc = document.querySelector("#nameSearch").value;

    return ({ desc, minPrice, maxPrice, categoryIds });
}
sessionStorage.setItem("categoryIds", JSON.stringify([]))
console.log((JSON.stringify(sessionStorage.getItem("orderItems"))))
sessionStorage.setItem("orderItems",(sessionStorage.getItem("orderItems") ? JSON.stringify(JSON.parse(sessionStorage.getItem("orderItems"))): JSON.stringify([])))


const filterProducts = async () => {
    const obj = GetDataFromDocumentForFilter()
    let url = `../api/Product`
    if (obj.desc || obj.minPrice || obj.maxPrice || obj.categoryIds.length!=0) 
        url += '?'
        if (obj.desc)
            url += `&desc=${obj.desc}`
        if (obj.minPrice)
            url += `&minPrice=${obj.minPrice}`
        if (obj.maxPrice)
            url += `&maxPrice=${obj.maxPrice}`
    if (obj.categoryIds.length != 0)
        for (let i = 0; i < obj.categoryIds.length;i++)
            url += `&categoryIds=${obj.categoryIds[i]}`
          try {
              const Products = await fetch(url, {
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
              const data = await Products.json();
                      console.log(data)
              if (!Products.ok) {
                  throw new Error(`HTTP error! status:${Products.status}`);
                      }
              else
                  drawProducts(data) 

                  } catch (error) {
                      console.log(error)
                  }
              }
const drawTemplate = async (product) => {
    let tmp = document.getElementById("temp-card");
    let cloneProduct = tmp.content.cloneNode(true)
    cloneProduct.querySelector("img").src = "../img/" + product.imgUrl
    cloneProduct.querySelector("h1").textContent = product.productName
    cloneProduct.querySelector(".price").innerText = product.price
    cloneProduct.querySelector(".description").innerText = product.description
    cloneProduct.getElementById("bag").addEventListener('click', () => { addToBag(product) }) 
    document.getElementById("PoductList").appendChild(cloneProduct)
    
}
const drawProducts = async (products) => {
    document.getElementById("PoductList").innerHTML = ""
    for (let i = 0; i < products.length; i++) {
        drawTemplate(products[i])
    }
}
const getCategory = async () => {
    try {
        const Categories = await fetch('../api/Category', {
            method: "GET",
            headers: {
                'Content-type': 'application/json'
            }
        });
        const data = await Categories.json();
        console.log(data)
        if (!Categories.ok) {
            throw new Error(`HTTP error! status:${Categories.status}`);
        }
        else
            drawCategories(data)
    }
    catch (error) {
        console.log(error)
    }
    }
const drawCategories = (categories)=>{
    for (let i = 0; i < categories.length; i++) {
        let tmp = document.getElementById("temp-category");
        let cloneCategory = tmp.content.cloneNode(true)
        cloneCategory.querySelector(".OptionName").textContent = categories[i].categoryName;
        cloneCategory.querySelector(".opt").addEventListener('change', () => { filterCategory(categories[i])})
        document.getElementById("categoryList").appendChild(cloneCategory) 
    }
}
const filterCategory = (category) => {
    let categories = JSON.parse(sessionStorage.getItem("categoryIds"))
    let a = categories.indexOf(category.categoryId)
    a == -1 ? categories.push(category.categoryId) : categories.splice(a, 1)
    sessionStorage.setItem("categoryIds", JSON.stringify(categories))
    filterProducts()
}

const addToBag = (product) => {
    let items = JSON.parse(sessionStorage.getItem("orderItems"))  
    items.push(product)
    sessionStorage.setItem("orderItems", JSON.stringify(items))
    document.getElementById("ItemsCountText").textContent = items.length
}

addEventListener('load', () => {
    filterProducts()
    getCategory()
   let items = JSON.parse(sessionStorage.getItem("orderItems"))
    document.getElementById("ItemsCountText").textContent = items.length
})

UpdateUser=() => {
    if (sessionStorage.getItem("id") != null)
        window.location.href = 'Update.html'
    else
        alert("אינך משתמש, אי אפשר לעדכן פרטים!")
}
   

