import React, {useEffect} from "react";


const Index = () => {
  useEffect(() =>{
    document.title ="Khu vực quản trị Admin";
  }, []);
  return (
    <>
      <h1>Đây là khu vực của người quản trị</h1>
    </>
  );
}

export default Index;