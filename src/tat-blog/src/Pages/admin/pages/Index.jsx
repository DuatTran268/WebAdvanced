import { faGlobe, faList, faLock, faUser } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { getDashboard} from "../../../Services/Dashboard";


const Index = () => {
  const [data, setData] = useState({});

  useEffect(() => {
    document.title = "Khu vực quản trị Admin";
    dataDashboard();

    async function dataDashboard(){
      const response = await getDashboard();
      if (response){
        setData(response);
      }else{
        setData({});
      }
    }
    
  }, []);

  return (
    <>
      <div className="row card-body justify-content-between">
        <div class="col-md-5 card mt-5">
          <div class="row align-items-center">
            <div class="col-4">
            <FontAwesomeIcon icon={faGlobe} fontSize={100}/>
            </div>
            <div class="col-8">
              <div class="text-primary text-uppercase">Tổng số bài viết</div>
              <h3 class="text-danger fw-bold">
                {data.postCount}
              </h3>
            </div>
          </div>
        </div>

        <div class="col-md-5 card mt-5">
          <div class="row align-items-center">
            <div class="col-4">
            <FontAwesomeIcon icon={faLock} fontSize={100}/>
            </div>
            <div class="col-8" >
              <div class="text-primary text-uppercase">
                Số bài viết chưa xuất bản
              </div>
              <h3 class="text-danger fw-bold">
              </h3>
            </div>
          </div>
        </div>
        
        <div class="col-md-5 card mt-5">
          <div class="row align-items-center">
            <div class="col-4">
            <FontAwesomeIcon icon={faList} fontSize={100}/>
            </div>
            <div class="col-8">
              <div class="text-primary text-uppercase">Số lượng chủ đề</div>
              <h3 class="text-danger fw-bold"></h3>
            </div>
          </div>
        </div>

        <div class="col-md-5 card mt-5">
          <div class="row align-items-center">
            <div class="col-4">
            <FontAwesomeIcon icon={faUser} fontSize={100}/>
            </div>
            <div class="col-8">
              <div class="text-primary text-uppercase">Số lượng tác giả</div>
              <h3 class="text-danger fw-bold"></h3>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Index;
