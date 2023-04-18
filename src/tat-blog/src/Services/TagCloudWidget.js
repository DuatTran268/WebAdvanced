import { get_api, put_api } from "./Methods";

export async function getTagClould(){

  return get_api (`https://localhost:7247/api/tags`)

}

export async function getTagBySlug(urlSlug = '', pageSize = 5, pageNumber = 1){

  return get_api (`https://localhost:7247/api/tags/${urlSlug}?PageSize=${pageSize}&PageNumber=${pageNumber}`)

  
}

export async function getFilterTag(){
  return get_api (`https://localhost:7247/api/tags`)
}


export function getTagFilter (name = '',
pageSize = 10, pageNumber =1, sortColumn = '', sortOrder = ''){
  let url = new URL(`https://localhost:7247/api/tags/tagRequire`);
  name  !== '' && url.searchParams.append('Name', name);
  sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
  sortOrder !== '' && url.searchParams.append('SortOrder', sortColumn);
  url.searchParams.append('PageSize', pageSize);
  url.searchParams.append('PageNumber', pageNumber);

  return get_api(url.href);
}

export async function getTagById(id = 0){
  if(id > 0) {
    return get_api(`https://localhost:7247/api/tags/${id}`)
  }
}

export async function puUpdateTag(id = 0){
  if (id > 0){
    return put_api (`https://localhost:7247/api/tags/${id}`)
  }
}