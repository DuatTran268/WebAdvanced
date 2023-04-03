import axios from "axios";



export async function getPosts (keyword = '', pageSize = 5, pageNumber = 1, sortColumn = '', sortOrder = ''){
  try {
    // // call api
    const response = await axios.get(`https://localhost:7247/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);

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