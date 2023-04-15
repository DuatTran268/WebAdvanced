/* eslint-disable no-undef */
import React, { useEffect, useState } from "react";
import { getPostFilter } from "../../../../Services/BlogRepository";
import Loading from "../../../../Components/Loading";
import { Table } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import PostFilerPane from "../../../../Components/admin/PostFilterPane";
import { useSelector } from "react-redux";


const Posts = () => {
  const [postsList, setPostList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true),
  postFilter = useSelector(state => state.postFilter);

  let {id} = useParams,
    p = 1,
    ps = 10;

  useEffect(() => {
    document.title = "Quản lý bài viết";

    getPostFilter(postFilter.keyword, postFilter.authorId, postFilter.categoryId, postFilter.year, postFilter.month, ps, p).then(data => {
      if (data) {
        setPostList(data.items);
        console.log("data: ", data);
      } else {
        setPostList([]);
      }
      setIsVisibleLoading(false);
    });

  }, [postFilter.keyword, postFilter.authorId, postFilter.categoryId, postFilter.year, postFilter.month, p, ps]);

  return (
    <>
      <h1>Đây là khu vực quản lý bài viết {id}</h1>
      <PostFilerPane/>
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tiêu đề</th>
              <th>Tác giả</th>
              <th>Chủ đề</th>
              <th>Xuất bản</th>
            </tr>
          </thead>
          <tbody>
            {postsList.length > 0 ? (
              postsList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link
                      to={`/admin/posts/edit/${item.id}`}
                      className="text-bold"
                    >
                      {item.title}
                    </Link>
                    <p className="text-muted">{item.shortDescription}</p>
                  </td>
                  <td>{item.author.fullName}</td>
                  <td>{item.category.name}</td>
                  <td>{item.published ? "Có" : "Không"}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>
                  <h4 className="text-danger text-center">
                    Không tìm thấy bài viết nào
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
export default Posts;
