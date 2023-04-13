import { get_api } from "./Methods";

export async function getCategories(){
  return get_api (`https://localhost:7247/api/categories`);
}

// tập tin widget.js chứa các hàm gọi api dành cho các widget ở sidebar
export async function getCategoryBySlug (urlSlug = '', pageSize = 5, pageNumber = 1){

  return get_api (`https://localhost:7247/api/categories/${urlSlug}?PageSize=${pageSize}&PageNumber=${pageNumber}`)

 
}

