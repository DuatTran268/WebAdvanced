import axios from "axios";

export async function getTagClould(){
  try {
    const response = await axios.get(`https://localhost:7247/api/tags`);

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

