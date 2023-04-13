import { get_api } from "./Methods";

export async function getRandomPost(){

  return get_api (`https://localhost:7247/api/posts/random/5`)
  
}

