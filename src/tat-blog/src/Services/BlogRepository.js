import axios from "axios";



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


export async function getPostBySlug (urlSlug = ''){
  try {
    const response = await axios.get(`https://localhost:7247/api/posts/byslug/${urlSlug}`);
    const data = response.data
    console.log(data)
    if (data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null
  }
}