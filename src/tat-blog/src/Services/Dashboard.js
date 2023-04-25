import { get_api } from "./Methods";

export async function getDashboard(){
  
  return get_api (`https://localhost:7247/api/dasboards`)

}