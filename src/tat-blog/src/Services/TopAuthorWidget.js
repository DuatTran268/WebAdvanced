import { get_api, post_api, delete_api, put_api } from "./Methods";


export async function getTopAuthor(){
  return get_api (`https://localhost:7247/api/authors/best/4`)
  
}

export async function getPostByAuthorSlug (urlSlug = ''){

  return get_api (`https://localhost:7247/api/authors/${urlSlug}`)
}



export async function getFilterAuthor(){
  return get_api (`https://localhost:7247/api/authors/notrequired`)

}

export function getAuthorFilter (name = '',
pageSize = 10, pageNumber =1, sortColumn = '', sortOrder = ''){
  let url = new URL(`https://localhost:7247/api/authors`);
  name  !== '' && url.searchParams.append('Name', name);
  sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
  sortOrder !== '' && url.searchParams.append('SortOrder', sortColumn);
  url.searchParams.append('PageSize', pageSize);
  url.searchParams.append('PageNumber', pageNumber);

  return get_api(url.href);
}


export async function getAuthorById(id = 0){
  if(id > 0){
    return get_api(`https://localhost:7247/api/authors/${id}`)
  }
}

export async function updateAuthor(id = 0, formData){
  return put_api (`https://localhost:7247/api/authors/${id}`, formData)
}

export async function deleteAuthor(id = 0){
  return delete_api (`https://localhost:7247/api/authors/${id}`)

}