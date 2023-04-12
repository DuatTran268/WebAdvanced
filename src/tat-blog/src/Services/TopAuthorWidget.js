import axios from "axios";


export async function getTopAuthor(){
  try {
    const response = await axios.get(`https://localhost:7247/api/authors/best/4`);

    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getPostByAuthorSlug (urlSlug = ''){

  try {
    const response = await axios.get(`https://localhost:7247/api/authors/${urlSlug}`);

    const data = response.data
    if (data.isSuccess)
      return data.result;
    else
      return null;
  } catch (error) {
    
  }
}