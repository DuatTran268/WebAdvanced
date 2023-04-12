import React, { useEffect } from "react";
import { Image } from "react-bootstrap";

const About = () => {
  useEffect(() => {
    document.title = "Giới thiệu";
  }, []);

  return (
    <>
      <div class="container">
        <div class="card-body text-center">
          <h1>Giới thiệu về chúng tôi</h1>
          <h3 class="text-primary">
            Một công ty về phần mềm và chuyên viết tạo các blog
          </h3>
          <p class="mt-5">
            People who find you online want to know about your company’s
            mission, vision, and, of course, how you can help them solve their
            problems. And, an impactful and memorable first impression can help
            you stand out in the digital world. How can you make sure that they
            have all that information at their fingertips? Well, a company
            profile is the perfect tool for the job. You can justify a higher
            price point for your products and services, if you go into details
            about your production values or ethically-sourced materials. For
            instance, Starbucks’ coffee may not necessarily be better than
            Dunkin’ Donuts’ coffee, but because Starbucks goes into details
            about its high-quality ingredients, it immediately creates the sense
            that you’ll be paying a little more for a "better" product.
          </p>
          <div class="card-footer">
            <Image src="https://i.ytimg.com/vi/BTB86HeZVwk/sddefault.jpg" />
          </div>
        </div>
      </div>
    </>
  );
};

export default About;
