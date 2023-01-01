# Guid Math
Performing basic mathematic operations with GUIDs regardless of its usefulness.

## Why?
My theory has always been that if Guids (UUID) can be sorted (ordered) then they can be incremented. If they can be sorted and incremented then this means there is a potential attack vector for sites that expose their guids in the URL. Guids are not as obscure as people want to make them out to be and I actually don't think it's safe to expose them. If you can find two guids from a system and subtract them, then you can iterate between the two of them and the number isn't infinity like some people like to believe. This library allows me to...
- Add
- Subtract
- Multiply
- Divide

... Guids regardless of how useful it is. This is more of a theoretical exercise.

## Testing
Testing guids is very difficuilt because they are very large numbers. I have done my best to test using some set theory and mostly basic algebraic concepts.

# Sequential Guid Generation
12/31/2022 Coincidentally as people are migrating from Windows docker containers to Linux docker containers due to the adoption of dot net core plus (dot net 6+) they can no longer rely on using the `rpcrt4.dll` which is a Windows-centric DLL. Once again, for fun, I have created a Sequential Guid generator which just utilizes the Guid Math I was working with. The performance surprised me as I wasn't expecing my library to be performant. If you are trying to generate more than one million sequential Guids at a time, then this is not the library for you. However, if it is just to generate a couple hundred it's very fast surprisingly. This is a dot net 6 library that has no dependency on Windows.
