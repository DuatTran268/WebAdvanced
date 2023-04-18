import axios from "axios";

import { get_api, post_api, delete_api } from "./Methods";

export async function getPosts (params){
  
  try {
    // // call api
    const response = await axios.get(`https://localhost:7247/api/posts`, {params});

    const data = response.data
    if(data.isSuccess){
      return data.result;
    }
    else {
      return null;
    }
  }
  catch(error){
    console.log('Error', error.message);
    return null
  }
}


export async function getPostsAdmin (keyword = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = ''){
  return get_api(`https://localhost:7247/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export async function getPostBySlug (urlSlug = ''){
  return get_api (`https://localhost:7247/api/posts/byslug/${urlSlug}`)

}

export async function getFilter() {
  return get_api (`https://localhost:7247/api/posts/get-filter`);
}


export function getPostFilter (keyword = '', authorId = '', categoryId = '', year='', month ='',
pageSize = 10, pageNumber =1, sortColumn = '', sortOrder = ''){
  let url = new URL(`https://localhost:7247/api/posts/get-posts-filter`);
  keyword  !== '' && url.searchParams.append('Keyword', keyword);
  authorId !== '' && url.searchParams.append('AuthorId', authorId);
  categoryId !== '' && url.searchParams.append('CategoryId', categoryId);
  year !== '' && url.searchParams.append('Year', year);
  month !== '' && url.searchParams.append('Month', month);
  sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
  sortOrder !== '' && url.searchParams.append('SortOrder', sortColumn);
  url.searchParams.append('PageSize', pageSize);
  url.searchParams.append('PageNumber', pageNumber);
  
  return get_api(url.href);

}

export async function getPostById(id = 0){
  if (id > 0)
    return get_api(`https://localhost:7247/api/posts/${id}`)
}

export function addOrUpdatePost(formData){
  return post_api(`https://localhost:7247/api/posts`, formData);
}

export async function deletePost(id = 0){
  return delete_api(`https://localhost:7247/api/posts/${id}`);
}