# GroceryCo Self-Checkout Kiosk System
A metaphorical supermarket company named "GroceryCo" hired a software developer to make a kiosk checkout system for their stores. The system lets customers choose to perform a self-checkout so that when they are feeling generous, they can let the people with reduced mobility go to cashiers while they use the system to checkout their items themselves.

> It's 2017! You cannot stop the progress of machines!
>
> -- <cite>GroceryCo CEO</cite>

## Table of Contents

- [Security](#security)
- [Install](#install)
- [Usage](#usage)
- [API](#api)
- [Contribute](#contribute)
- [License](#license)

## Security
Considering the complexity of a Security that works, its essential to implement a robust security. Otherwise, it's just a waste of time that could have been used for productivity. Knowing this is a timed project, I simply used most of my time to learn C# programming and developing the core functionalities of the system.

## Install
There's no installation necessary for this. To run the application, follow these steps:
- Using a command line, run the `GroceryCo` executable file.
- It will ask for the directory of the text file that will contain UPC "Bar Codes" that the Kiosk will use to scan. Enter the directory when prompted.
After that, it should scan through all the UPC codes and gets the appropriate information from a database and prints out a receipt.

## Contribute
PRs accepted.