import { delete_api, get_api, put_api } from "./Methods";

export async function getCategories(){
  return get_api (`https://localhost:7247/api/categories`);
}

// tập tin widget.js chứa các hàm gọi api dành cho các widget ở sidebar
export async function getCategoryBySlug (urlSlug = '', pageSize = 5, pageNumber = 1){

  return get_api (`https://localhost:7247/api/categories/${urlSlug}?PageSize=${pageSize}&PageNumber=${pageNumber}`)

 
}

export function getFilterCategory(name = '',
pageSize = 10, pageNumber =1, sortColumn = '', sortOrder = ''){
  let url = new URL(`https://localhost:7247/api/categories/listcate`);
  name  !== '' && url.searchParams.append('Name', name);
  sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
  sortOrder !== '' && url.searchParams.append('SortOrder', sortColumn);
  url.searchParams.append('PageSize', pageSize);
  url.searchParams.append('PageNumber', pageNumber);

  return get_api(url.href);
}

export async function getCategoryById(id = 0){
  if(id > 0){
    return get_api(`https://localhost:7247/api/categories/byid/${id}`)
  }
}

export async function updateCategory(id = 0){
  if (id > 0){
    return put_api (`https://localhost:7247/api/categories/${id}`);
  }
}

export async function deletCategory(id = 0){
  return delete_api (`https://localhost:7247/api/categories/${id}`);
}