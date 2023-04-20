import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import CategoryFilterPane from "../../../../Components/admin/CategoryPane";
import Loading from "../../../../Components/Loading";
import {
  getFilterCategory,
} from "../../../../Services/CategoriesWidgets";

const Categories = () => {
  const [categoryList, setCategoryList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    categoryFilter = useSelector(state => state.categoryFilter);

  let {id} = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý chủ đế";

    getFilterCategory(categoryFilter, ps, p).then((data) => {
      if (data) {
        console.log("data category: ", data);
        setCategoryList(data.items);
      } else {
        setCategoryList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [categoryFilter, p, ps]);

  return (
    <>
      <h1>Đây là khu vực quản lý chủ đề {id}</h1>

      {/* <CategoryFilterPane/> */}
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tên category</th>
              <th>Số bài viết</th>
              <th>Xoá</th>
            </tr>
          </thead>
          <tbody>
            {categoryList.length > 0 ? (
              categoryList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link to={`/admin/categories/edit/${item.id}`}>
                    {item.name}
                    </Link>
                    <p className="text-muted">{item.description}</p>
                  </td>
                  <td>
                    {item.postCount}
                  </td>
                  <td>Xoá đi</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={3}>
                  <h4 className="text-danger text-center">
                    Không tìm thấy category nào
                  </h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
    </>
  );
};

export default Categories;
