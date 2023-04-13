import axios from "axios";

import { get_api } from "./Methods";

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