import axios from "axios";

export async function getRandomPost(){
  try {
    const response = await axios.get(`https://localhost:7247/api/posts/random/5`);

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

